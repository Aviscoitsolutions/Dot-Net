using System.ServiceProcess;
using System.Threading;

namespace ExampleService.Srv
{
    public partial class Service : ServiceBase
    {
        private Server server;

        public Service()
        {
            InitializeComponent();
            this.server = new Server();
        }

        protected override void OnStart(string[] args)
        {
            this.server.Start();
        }

        protected override void OnStop()
        {
            this.server.Stop();
            while (this.server.IsActive())
            {
                Thread.Sleep(100);
            }
        }
    }
}