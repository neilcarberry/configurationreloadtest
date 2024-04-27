using Microsoft.Extensions.Options;

namespace configurationreloadtest
{
    public class singleyclass
    {
        private readonly IOptionsMonitor<TestObject> _testobjmonitor;
        public singleyclass(IOptionsMonitor<TestObject> testobjmonitor)
        {
            _testobjmonitor = testobjmonitor;
        }

        public string getpass()
        {
            return _testobjmonitor.CurrentValue.Password;
        }
    }
}
