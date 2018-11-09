using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.Messages
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
