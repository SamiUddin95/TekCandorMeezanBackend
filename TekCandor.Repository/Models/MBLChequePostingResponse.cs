namespace TekCandor.Repository.Models
{
    public class MBLChequePostingExt
    {
        public MBLFT MBLFT { get; set; } = new MBLFT();
    }

    public class MBLFT
    {
        public string StatusCode { get; set; } = string.Empty;
        public string StatusDesc { get; set; } = string.Empty;
        public string STAN { get; set; } = string.Empty;
        public HostData hostData { get; set; } = new HostData();
    }

    public class HostData
    {
        public HostFTResponse hostFTResponse { get; set; } = new HostFTResponse();
    }

    public class HostFTResponse
    {
        public string HostCode { get; set; } = string.Empty;
        public string HostDesc { get; set; } = string.Empty;
        public string TransactionID { get; set; } = string.Empty;
    }
}
