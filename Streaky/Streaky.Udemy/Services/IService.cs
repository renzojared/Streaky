namespace Streaky.Udemy.Services;

public interface IService
{
    Guid GetScoped();
    Guid GetSingleton();
    Guid GetTransient();
    void DoTask();
}

public class ServiceA : IService
{
    private readonly ILogger<ServiceA> logger;
    private readonly ServiceTransient serviceTransient;
    private readonly ServiceScoped serviceScoped;
    private readonly ServiceSingleton serviceSingleton;

    public ServiceA(ILogger<ServiceA> logger, ServiceTransient serviceTransient, ServiceScoped serviceScoped, ServiceSingleton serviceSingleton)
    {
        this.logger = logger;
        this.serviceTransient = serviceTransient;
        this.serviceScoped = serviceScoped;
        this.serviceSingleton = serviceSingleton;
    }

    public Guid GetTransient() { return serviceTransient.Guid; }
    public Guid GetScoped() { return serviceScoped.Guid; }
    public Guid GetSingleton() { return serviceSingleton.Guid; }

    public void DoTask()
    {
    }
}

public class ServiceB : IService
{
    public void DoTask()
    {
    }

    public Guid GetScoped()
    {
        throw new NotImplementedException();
    }

    public Guid GetSingleton()
    {
        throw new NotImplementedException();
    }

    public Guid GetTransient()
    {
        throw new NotImplementedException();
    }
}

public class ServiceTransient
{
    public Guid Guid = Guid.NewGuid();
}

public class ServiceScoped
{
    public Guid Guid = Guid.NewGuid();
}

public class ServiceSingleton
{
    public Guid Guid = Guid.NewGuid();
}