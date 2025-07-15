using System;

public class CustomerViewModel
{
    public int Id { get; set; }
    public int DealerId { get; set; }
    public string NationalIdentity { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Note { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public bool ActiveFlg { get; set; }
}
