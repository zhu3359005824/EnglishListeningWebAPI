namespace IDentity.WebAPI.Event
{
    public record UserCreatedEvent(Guid Id,string UserName,string Password,string PhoneNumber)
    {
    }
}
