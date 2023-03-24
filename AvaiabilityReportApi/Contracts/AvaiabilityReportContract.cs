using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace AvaiabilityReportApi.Contracts
{
    public class AvaiabilityReportContract
    {
        public string app_eui { get; set; }
        public Dc dc { get; set; }
        public Decoded decoded { get; set; }
        public string dev_eui { get; set; }
        public string devaddr { get; set; }
        public string downlink_url { get; set; }
        public int fcnt { get; set; }
        public List<Hotspot> hotspots { get; set; }
        public string id { get; set; }
        public Metadata metadata { get; set; }
        public string name { get; set; }
        public string payload { get; set; }
        public int payload_size { get; set; }
        public int port { get; set; }
        public string raw_packet { get; set; }
        public bool replay { get; set; }
        public long reported_at { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
    }

    public class Payload
    {
        public bool occupancy { get; set; }
    }

    public class Hotspot
    {
        public int channel { get; set; }
        public double frequency { get; set; }
        public int hold_time { get; set; }
        public string id { get; set; }
        public double lat { get; set; }
        public double @long { get; set; }
        public string name { get; set; }
        public long reported_at { get; set; }
        public int rssi { get; set; }
        public int snr { get; set; }
        public string spreading { get; set; }
        public string status { get; set; }
    }

    public class Metadata
    {
        public bool adr_allowed { get; set; }
        public bool cf_list_enabled { get; set; }
        public int multi_buy { get; set; }
        public string organization_id { get; set; }
        public List<object> preferred_hotspots { get; set; }
        public int rx_delay { get; set; }
        public int rx_delay_actual { get; set; }
        public string rx_delay_state { get; set; }
    }

    public class Dc
    {
        public int balance { get; set; }
        public int nonce { get; set; }
    }

    public class Decoded
    {
        public Payload payload { get; set; }
        public string status { get; set; }
    }

}
