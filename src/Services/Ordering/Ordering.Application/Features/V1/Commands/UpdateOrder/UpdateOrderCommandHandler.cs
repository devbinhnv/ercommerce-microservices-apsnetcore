using AutoMapper;
using Contracts.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.Application.Features.V1.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _repository;
        private readonly IValidator<UpdateOrderCommand> _validator;
        private readonly ISmtpEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateOrderCommandHandler(
            IOrderRepository repository,
            IValidator<UpdateOrderCommand> validator,
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

        private const string METHOD_NAME = nameof(UpdateOrderCommandHandler);
        public async Task<ApiResult<long>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"START: {METHOD_NAME} with order {request.Id}");
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (validationResult.Errors.Any())
                {
                    var errors = validationResult.Errors
                    .Select(e => $"Key: \"{e.PropertyName}\": {e.ErrorMessage}")
                    .ToList();
                    return new ApiErrorResult<long>(errors);
                }

                var orderEntity = await _repository.FindByCondition(o => o.Id == request.Id).SingleOrDefaultAsync();
                if (orderEntity == null)
                {
                    throw new NotFoundException(nameof(orderEntity), request.Id);
                }

                var updatingOrder = _mapper.Map(source: request, destination: orderEntity);
                await _repository.UpdateAsync(updatingOrder);
                await _repository.SaveChangeAsync();

                SendMailAsync(updatingOrder, cancellationToken);

                _logger.Information($"END: {METHOD_NAME} with order {request.Id}");
                return new ApiSuccessResult<long>(updatingOrder.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An occurring error in {METHOD_NAME} with order {request.Id}");
                throw;
            }
        }
        private async Task SendMailAsync(OrderEntity order, CancellationToken cancellationToken)
        {
            var emailRequest = new MailRequest
            {
                To = order.EmailAddress,
                Body = "Order was updated.",
                Subject = "Order was updated."
            };

            try
            {
                await _emailService.SendEmailAsync(emailRequest, cancellationToken);
                _logger.Information($"Sent  updated order to {order.EmailAddress}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Order {order.Id} failed due to an error with email service {ex.Message}");
                throw;
            }
        }
    }
}
