using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class banco
{
    static async Task Main()
    {
        string apiUrl = "https://air-sensor-ai--caiohenry18.replit.app/api/DHT";

        string firebaseUrl =
            "https://airsensorai-default-rtdb.firebaseio.com/sensores.json";

        using var http = new HttpClient();

        try
        {
            // 1️⃣ GET da sua API
            var response = await http.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var jsonApi = await response.Content.ReadAsStringAsync();

            // 2️⃣ POST no Firebase (cria histórico)
            var content = new StringContent(jsonApi, Encoding.UTF8, "application/json");

            var firebaseResponse = await http.PostAsync(firebaseUrl, content);
            firebaseResponse.EnsureSuccessStatusCode();

            Console.WriteLine("✅ Dados enviados para o Firebase com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Erro: " + ex.Message);
        }
    }
}