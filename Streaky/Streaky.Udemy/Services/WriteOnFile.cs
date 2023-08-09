namespace Streaky.Udemy.Services;

public class WriteOnFile : IHostedService
{
    private readonly IWebHostEnvironment env;
    private readonly string fileName = "file_1.txt";
    private Timer timer;

    public WriteOnFile(IWebHostEnvironment env)
    {
        this.env = env;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        Write("Proceso iniciado");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer.Dispose();
        Write("Proceso finalizado");
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        Write("Proceso en ejecucion: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
    }

    private void Write(string message)
    {
        var route = $@"{env.ContentRootPath}/wwwroot/{fileName}";
        using (StreamWriter writer = new StreamWriter(route, append: true))
        {
            writer.WriteLine(message);
        }
    }
}

