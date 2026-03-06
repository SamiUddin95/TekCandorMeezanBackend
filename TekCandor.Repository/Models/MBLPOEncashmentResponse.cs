namespace TekCandor.Repository.Models
{
    public class MBLPOLodgementExt
    {
        public MBLPayOrderEncashmentResponse MBLPayOrderEncashmentResponse { get; set; } = new MBLPayOrderEncashmentResponse();
    }

    public class MBLPayOrderEncashmentResponse
    {
        public string StatusCode { get; set; } = string.Empty;
        public string StatusDesc { get; set; } = string.Empty;
        public string STAN { get; set; } = string.Empty;
        public HostDataPO hostData { get; set; } = new HostDataPO();
    }

    public class HostDataPO
    {
        public HostPayOrderEncashmentResponse HostPayOrderEncashmentResponse { get; set; } = new HostPayOrderEncashmentResponse();
    }

    public class HostPayOrderEncashmentResponse
    {
        public string HostCode { get; set; } = string.Empty;
        public string HostDesc { get; set; } = string.Empty;
        public string TransactionID { get; set; } = string.Empty;
    }
}
