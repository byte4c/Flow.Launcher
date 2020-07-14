using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

namespace Flow.Launcher.Plugin.Time
{
    public class Main : IPlugin, IPluginI18n
    {
        private PluginInitContext context;

        static bool CopyToClipboard(string data)
        {
            try
            {
                Clipboard.SetText(data);
                return true;
            }
            catch (ExternalException)
            {
                MessageBox.Show("Copy failed, please try later");
                return false;
            }
        }

        public void Init(PluginInitContext context)
        {
            this.context = context;
        }

        public List<Result> Query(Query query)
        {
            return query.Terms.Length > 1 && long.TryParse(query.Terms[1], out var unixTime)
                ? GenerateTimeResults(DateTimeOffset.FromUnixTimeSeconds(unixTime))
                : GenerateTimeResults(null);
        }
        private List<Result> GenerateTimeResults(DateTimeOffset? dateTime)
        {
            var results = new List<Result>();

            if (!dateTime.HasValue)
            {
                results.Add(new Result
                {
                    Title = "Unix",
                    SubTitle = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                    Score = 100,
                    IcoPath = context.CurrentPluginMetadata.IcoPath,
                    Action = (_) => CopyToClipboard(DateTimeOffset.Now.ToUnixTimeSeconds().ToString())
                });
                results.Add(new Result
                {
                    Title = "Unix with millies",
                    SubTitle = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(),
                    Score = 100,
                    IcoPath = context.CurrentPluginMetadata.IcoPath,
                    Action = (_) => CopyToClipboard(DateTimeOffset.Now.ToUnixTimeSeconds().ToString())
                });
            }

            results.Add(new Result
            {
                Title = "ISO Format",
                SubTitle = (dateTime ?? DateTimeOffset.Now).ToString("s"),
                Score = 100,
                IcoPath = context.CurrentPluginMetadata.IcoPath,
                Action = (_) => CopyToClipboard((dateTime ?? DateTimeOffset.Now).ToString("s"))
            });

            return results;
        }

        public string GetTranslatedPluginTitle()
        {
            return context.API.GetTranslation("flowlauncher_plugin_time_plugin_name");
        }
        public string GetTranslatedPluginDescription()
        {
            return context.API.GetTranslation("flowlauncher_plugin_time_plugin_description");
        }
    }
}
