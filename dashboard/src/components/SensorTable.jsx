export default function SensorTable({ readings }) {
  const sorted = [...readings].reverse() // newest first

  return (
    <div className="table-wrapper">
      <table className="sensor-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Dispositivo</th>
            <th>Data/Hora</th>
            <th>Umidade (%)</th>
            <th>Valor Bruto</th>
          </tr>
        </thead>
        <tbody>
          {sorted.map((r, i) => (
            <tr key={r._id ?? i}>
              <td>{readings.length - i}</td>
              <td><code>{r.DeviceId}</code></td>
              <td>{new Date(r.Timestamp * 1000).toLocaleString('pt-BR')}</td>
              <td>
                <div className="moisture-bar-container">
                  <div
                    className="moisture-bar"
                    style={{ width: `${Math.min(r.SoilMoisturePercent, 100)}%` }}
                  />
                  <span>{r.SoilMoisturePercent?.toFixed(1)}%</span>
                </div>
              </td>
              <td>{r.SoilMoistureRaw?.toFixed(0)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
