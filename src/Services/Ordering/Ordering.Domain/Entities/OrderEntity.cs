using Contracts.Domains;
using Ordering.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities;

[Table("Order")]
public class OrderEntity : EntityAuditBase<long>
{
    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string UserName { get; set; }

    [Column(TypeName ="decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string LastName { get; set; }

    [Required]
    [Column(TypeName ="nvarchar(250)")]
    public string EmailAddress { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string ShippingAddress { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string InvoiceAddress { get; set; }

    [Required]
    public EOrderStatus Status { get; set; }
}
