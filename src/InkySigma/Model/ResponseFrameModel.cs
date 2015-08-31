namespace InkySigma.Model
{
    public class ResponseFrameModel
    {
        public bool Succeeded { get; set; } = false;
        public string Exception { get; set; }

        public dynamic Payload { get; set; }
    }
}
