namespace DevicesAndProblems.App.Messages
{
    public class UpdateListMessage
    {
        public bool CloseScreen { get; set; }

        public UpdateListMessage(bool closeScreen)
        {
            CloseScreen = closeScreen;
        }
    }
}
