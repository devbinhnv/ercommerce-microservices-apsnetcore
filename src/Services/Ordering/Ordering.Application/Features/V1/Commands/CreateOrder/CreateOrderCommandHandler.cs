using AutoMapper;
using Contracts.Services;
using FluentValidation;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.Application.Features.V1.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IOrderRepository _repository;
    private readonly IValidator<CreateOrderCommand> _validator;
    private readonly ISmtpEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateOrderCommandHandler(
        IOrderRepository repository,
        IValidator<CreateOrderCommand> validator,
        ISmtpEmailService emailService,
        IMapper mapper,
        ILogger logger)
    {
        _repository = repository;
        _validator = validator;
        _emailService = emailService;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.Information($"START: CreateOrderCommandHandler");
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
            {
                var errors = validationResult.Errors
                    .Select(e => $"Key: \"{e.PropertyName}\": {e.ErrorMessage}")
                    .ToList();
                return new ApiErrorResult<long>(errors);
            }

            var newOrder = _mapper.Map<OrderEntity>(request);
            await _repository.CreateAsync(newOrder);
            await _repository.SaveChangeAsync();
            SendMailAsync(newOrder, cancellationToken);

            _logger.Information($"END: CreateOrderCommandHandler");
            return new ApiSuccessResult<long>(newOrder.Id, "Create succeed");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"An occurring error in CreateOrderCommandHandler");
            throw;
        }
    }

    private async Task SendMailAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        var emailRequest = new MailRequest
        {
            To = order.EmailAddress,
            Body = "Order was created.",
            Subject = "Order was created."
        };

        try
        {
            await _emailService.SendEmailAsync(emailRequest, cancellationToken);
            _logger.Information($"Sent  created order to {order.EmailAddress}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Order {order.Id} failed due to an error with email service {ex.Message}");
            throw;
        }
    }
}
