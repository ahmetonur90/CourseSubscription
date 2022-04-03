namespace Subscription.Test
{
    public class ObjectActionResultModel
    {
        public bool success { get; set; }
        public Message message { get; set; }
        public Data[] data { get; set; }
    }

    public class Message
    {
        public string statu { get; set; }
        public object header { get; set; }
        public object content { get; set; }
    }

    public class Data
    {
        public string training_name { get; set; }
        public string training_code { get; set; }
        public string course_name { get; set; }
        public string course_code { get; set; }
        public string training_month { get; set; }
        public string user_name { get; set; }
        public string user_gender { get; set; }
        public string user_email { get; set; }
    }

}
