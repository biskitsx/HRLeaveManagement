namespace HR.LeaveManagement.Application.Models.Email
{
    public class EmailSetting
    {
        
        public string ApiKey {get; set;} = string.Empty;
        public string FromAddress {get; set;} = string.Empty;
        public string FromName {get; set;} = string.Empty;
    }
}