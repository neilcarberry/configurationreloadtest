using Microsoft.Extensions.Options;

namespace configurationreloadtest
{
    public class ScopyClass
    {
        private readonly IOptionsMonitor<TestObject> _testobjmonitor;
        public ScopyClass(IOptionsMonitor<TestObject> testobjmonitor)
        {
            _testobjmonitor = testobjmonitor;
        }

        public string getpass()
        {
            return _testobjmonitor.CurrentValue.Password;
        }
    }
}
