using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Logger
{
    public static class LogManager
    {
        private const string NLogConfigFileName = "NLog.config";
        private const string DefaultConfig = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
        	"<nlog xmlns=\"http://www.nlog-project.org/schemas/NLog.xsd\"\n      " +
        	"xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\n        " +
            "autoReload=\"true\">\n    " +
        	"<targets>\n        " +
        	"<target name=\"logconsole\" xsi:type=\"Console\" />\n    " +
        	"</targets>\n    " +
        	"<rules>\n        " +
        	"<logger name=\"*\" minlevel=\"Info\" writeTo=\"logconsole\" />\n        " +
        	"</rules>\n" +
        	"</nlog>";

        private static readonly Assembly _entryAssembly = Assembly.GetEntryAssembly();

        private static readonly string _logName = _entryAssembly.GetName().Name;
        private static readonly string _appRoot = Path.GetDirectoryName(_entryAssembly.Location);

        private static readonly string _configFilePath = Path.Combine(_appRoot, NLogConfigFileName);
        private static readonly string _logBaseDirectory;

        public static Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();
        public static Dictionary<(ILogger logger, string LogType), IClassLogger> _classLoggers =
        new Dictionary<(ILogger LogName, string LogType), IClassLogger>();

        static LogManager()
        {
            try{
                _logBaseDirectory = Environment.GetEnvironmentVariable("AutoHomeLogFolder") ?? "/Users/Shared/AutoHome/Logs";
            }
            catch(Exception){
                _logBaseDirectory = "/Users/Shared/AutoHome/Logs";
            }


            if (!File.Exists(_configFilePath))
            {
                using (var configFile = File.Create(_configFilePath))
                {
                    using (var configWriter = new StreamWriter(configFile))
                    {
                        configWriter.WriteLine(DefaultConfig);
                    }
                }
            }

            Directory.CreateDirectory(_logBaseDirectory);
        }

        public static ILogger GetLogger(string logName = null)
        {
            logName = logName ?? _logName;
            if (!_loggers.TryGetValue(logName, out ILogger logger))
            {
                if (!NLog.LogManager.Configuration.AllTargets.Any(t => t.Name == logName))
                {
                    XDocument doc;
                    using (var configFile = File.Open(_configFilePath, FileMode.Open))
                    {
                        doc = XDocument.Load(configFile);
                    }

                    var ns = doc.Root.Name.Namespace;
                    var xsi = doc.Root.GetNamespaceOfPrefix("xsi");



                    var target = new XElement(ns + "target");

                    target.Add(new XAttribute("name", logName));
                    target.Add(new XAttribute(xsi + "type", "File"));
                    target.Add(new XAttribute("fileName", Path.Combine(_logBaseDirectory, $"{logName}.log")));
                    target.Add(new XAttribute("layout","${date:format=O}|${logger}|${threadid}|${level:uppercase=true}|${message}"));

                    var targets = doc.Root.Element(ns + "targets");

                    targets.Add(target);

                    using (var newConfigFile = File.Open(_configFilePath, FileMode.Truncate))
                    {
                        doc.Save(newConfigFile);
                    }
                }

                if (!NLog.LogManager.Configuration.LoggingRules.Any(lr => lr.RuleName == logName))
                {
                    XDocument doc;
                    using (var configFile = File.Open(_configFilePath, FileMode.Open))
                    {
                        doc = XDocument.Load(configFile);
                    }

                    var ns = doc.Root.Name.Namespace;
                    var xsi = doc.Root.GetNamespaceOfPrefix("xsi");

                    var rule = new XElement(ns + "rule");

                    rule.Add(new XAttribute("name", logName));
                    rule.Add(new XAttribute("minlevel", "Info"));
                    rule.Add(new XAttribute("writeTo", logName));

                    var targets = doc.Root.Element(ns + "rules");

                    targets.Add(rule);

                    using (var newConfigFile = File.Open(_configFilePath, FileMode.Truncate))
                    {
                        doc.Save(newConfigFile);
                    }
                }

                _loggers[logName] = logger = new Logger(NLog.LogManager.GetLogger(logName));
            }
            return logger;
        }

        public static IClassLogger GetClassLogger<TClass>(string logName = null)
        {
            return GetClassLogger(typeof(TClass).Name, logName);
        }

        public static IClassLogger GetClassLogger(string logName = null)
        {
            var stackTrace = Environment.StackTrace;

            var correctLine = stackTrace.Split('\n')[2];

            var correctPortion = correctLine.Substring(6, correctLine.IndexOf('/') - 6);

            var indexFirstPeriod = correctPortion.IndexOf('.');

            var classNameWithNamespace = correctPortion.Substring(
                indexFirstPeriod + 1,
                correctPortion.LastIndexOf('.') - (indexFirstPeriod + 1))
                .TrimEnd('.');

            var classSplit = classNameWithNamespace.Split('.');

            var className = classSplit[classSplit.Length - 1];

            return GetClassLogger(className, logName);
        }

        private static IClassLogger GetClassLogger(string className, string logName)
        {
            var logger = GetLogger(logName);

            if (!_classLoggers.TryGetValue((logger, className), out IClassLogger classLogger))
            {
                _classLoggers[(logger, className)] = classLogger = new ClassLogger(logger, className);
            }

            return classLogger;
        }
    }
}
