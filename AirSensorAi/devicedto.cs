using Google.Cloud.Firestore;

[FirestoreData]
public class DeviceDto
{
    [FirestoreProperty]
    public String DeviceId { get; set; }
    [FirestoreProperty]
    public long Timestamp { get; set; }
    [FirestoreProperty]
    public float SoilMoistureRaw { get; set; }
    [FirestoreProperty]
    public float SoilMoisturePercent { get; set; }
}