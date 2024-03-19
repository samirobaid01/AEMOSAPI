namespace AEMOSAPI.DTO
{
    public class OrganizationDTO
    {
        // org.Id, org.ParentId, org.Name, org.Zip, org.Detail, org.Address, org.IsParent, org.Email, org.ContactNumber
        public long Id { set; get; }
        public long ParentId { set; get; }
        public string? Name { set; get; }
        public string? Detail { set; get; }
        public string? Address { set; get; }
        public bool? IsParent { set; get; }
        public string? Email { set; get; }
        public string? ContactNumber { set; get; }
    }
}
