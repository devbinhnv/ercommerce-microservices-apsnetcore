using AutoMapper;
using Customer.API.Entities;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Customer;

namespace Customer.API.Services;

public class CustomerService(
    ICustomerRepository customerRepository, 
    IMapper mapper)
    : ICustomerService
{
    public async Task<IResult> GetCustomersAsync()
    {
        try
        {
            var customers = await customerRepository.FindAll().ToListAsync();
            var customerDtos = mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Results.Ok(customerDtos);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred while retrieving customers: {ex.Message}");
        }
    }

    public async Task<IResult> GetCustomerByUserNameAsync(string username)
    {
        try
        {
            var customerEntity = await customerRepository.GetCustomerByUserNameAsync(username);
            if (customerEntity == null)
                return Results.NotFound($"Customer with username '{username}' not found");

            var customerDto = mapper.Map<CustomerDto>(customerEntity);
            return Results.Ok(customerDto);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred while retrieving customer: {ex.Message}");
        }
    }

    public async Task<IResult> CreateCustomerAsync(CreateCustomerDto createDto)
    {
        try
        {
            var existingCustomer = await customerRepository.GetCustomerByUserNameAsync(createDto.UserName);
            if (existingCustomer != null)
                return Results.Conflict($"Customer with username '{createDto.UserName}' already exists");

            var customerEntity = mapper.Map<CustomerEntity>(createDto);
            await customerRepository.CreateAsync(customerEntity);
            await customerRepository.SaveChangeAsync();

            var customerDto = mapper.Map<CustomerDto>(customerEntity);
            return Results.Ok(customerDto);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred while creating customer: {ex.Message}");
        }
    }

    public async Task<IResult> UpdateCustomerAsync(int id, UpdateCustomerDto updaterDto)
    {
        try
        {
            if (updaterDto == null)
                return Results.BadRequest("Update data cannot be null");

            var customerEntity = await customerRepository
                .FindByCondition(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (customerEntity == null)
                return Results.NotFound($"Customer with ID {id} not found");

            var updateCustomer = mapper.Map(updaterDto, customerEntity);

            await customerRepository.UpdateAsync(customerEntity);
            await customerRepository.SaveChangeAsync();

            var updatedCustomerDto = mapper.Map<CustomerDto>(customerEntity);
            return Results.Ok(updatedCustomerDto);
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred while updating customer: {ex.Message}");
        }
    }

    public async Task<IResult> DeleteCustomerAsync(int id)
    {
        try
        {
            var customerEntity = await customerRepository
                .FindByCondition(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (customerEntity == null)
                return Results.NotFound($"Customer with ID {id} not found");

            await customerRepository.DeleteAsync(customerEntity);
            await customerRepository.SaveChangeAsync();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem($"An error occurred while deleting customer: {ex.Message}");
        }
    }
}
