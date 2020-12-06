using System;

namespace ZapanControls.Controls.Calendar.Common
{
    public class Appointment : BindableObject
    {
        private string subject;
        public string Subject
        {
            get { return subject; }
            set
            {
                if (subject != value)
                {
                    subject = value;
                    RaisePropertyChanged(nameof(Subject));
                }
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    RaisePropertyChanged(nameof(Location));
                }
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    startTime = value;
                    RaisePropertyChanged(nameof(StartTime));
                }
            }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set
            {
                if (endTime != value)
                {
                    endTime = value;
                    RaisePropertyChanged(nameof(EndTime));
                }
            }
        }

        private string body;
        public string Body
        {
            get { return body; }
            set
            {
                if (body != value)
                {
                    body = value;
                    RaisePropertyChanged(nameof(Body));
                }
            }
        }

        public override string ToString()
        {
            return Subject;
        }
    }
}
