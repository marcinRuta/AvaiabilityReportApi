using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace AvaiabilityReportApi.Contracts
{
    public class AvaiabilityReportTTNContract
    {
        public EndDeviceIds end_device_ids { get; set; }
        public List<string> correlation_ids { get; set; }
        public DateTime received_at { get; set; }
        public UplinkMessage uplink_message { get; set; }
    }

    public class ApplicationIds
    {
        public string application_id { get; set; }
    }

    public class EndDeviceIds
    {
        public string device_id { get; set; }
        public ApplicationIds application_ids { get; set; }
        public string dev_eui { get; set; }
        public string join_eui { get; set; }
        public string dev_addr { get; set; }
    }

    public class GatewayIds
    {
        public string gateway_id { get; set; }
        public string eui { get; set; }
    }

    public class RxMetadata
    {
        public GatewayIds gateway_ids { get; set; }
        public long timestamp { get; set; }
        public int rssi { get; set; }
        public int channel_rssi { get; set; }
        public double snr { get; set; }
        public string uplink_token { get; set; }
        public int channel_index { get; set; }
        public DateTime received_at { get; set; }
    }

    public class DataRate
    {
        public LoRa lora { get; set; }
    }

    public class LoRa
    {
        public int bandwidth { get; set; }
        public int spreading_factor { get; set; }
        public string coding_rate { get; set; }
    }

    public class Settings
    {
        public DataRate data_rate { get; set; }
        public string frequency { get; set; }
        public long timestamp { get; set; }
    }

    public class DecodedPayload
    {
        public bool occupancy { get; set; }
    }

    public class UplinkMessage
    {
        public string session_key_id { get; set; }
        public int f_port { get; set; }
        public int f_cnt { get; set; }
        public string frm_payload { get; set; }
        public DecodedPayload decoded_payload { get; set; }
        public List<RxMetadata> rx_metadata { get; set; }
        public Settings settings { get; set; }
        public DateTime received_at { get; set; }
        public string consumed_airtime { get; set; }
        public NetworkIds network_ids { get; set; }
    }

    public class NetworkIds
    {
        public string net_id { get; set; }
        public string tenant_id { get; set; }
        public string cluster_id { get; set; }
        public string cluster_address { get; set; }
    }
}
