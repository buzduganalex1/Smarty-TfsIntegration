namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.Common;

    public class TfsService
    {
        private readonly string uri;

        private readonly string personalAccessToken;

        private readonly string project;

        public TfsService()
        {
            this.uri = "https://abuzduga.visualstudio.com/";
            this.personalAccessToken = "cfbgbrbpsmivovtt73wcmccu6nwu6is2eqo5h7czt4eddlxb3bda";
            this.project = "MyFirstProject";
        }

        public List<SmartyWorkItem> GetWorkItemsForUser(string email)
        {
            var smartyWorkItems = new List<SmartyWorkItem>();

            var credentials = new VssBasicCredential(string.Empty, this.personalAccessToken);

            var wiql = new Wiql
            {
                Query = $"Select [State], [Title], [System.AssignedTo], [Microsoft.VSTS.Scheduling.RemainingWork], [Microsoft.VSTS.Scheduling.CompletedWork] From WorkItems Where [System.TeamProject] = '{this.project}' And [System.AssignedTo] = '{email}' And [System.State] <> \'Closed\' Order By [State] Asc, [Changed Date] Desc"
            };

            using (var workItemTrackingHttpClient = new WorkItemTrackingHttpClient(new Uri(this.uri), credentials))
            {
                var workItemQueryResult = workItemTrackingHttpClient.QueryByWiqlAsync(wiql).Result;

                if (workItemQueryResult.WorkItems.Count() != 0)
                {

                    var list = new List<int>();

                    foreach (var item in workItemQueryResult.WorkItems)
                    {
                        list.Add(item.Id);
                    }

                    var arr = list.ToArray();

                    var fields = new List<string>()
                                     {
                                         "System.Id",
                                         "System.Title",
                                         "System.State",
                                         "System.AssignedTo",
                                         "Microsoft.VSTS.Scheduling.RemainingWork",
                                         "Microsoft.VSTS.Scheduling.CompletedWork"
                                     };

                    var workItems = workItemTrackingHttpClient.GetWorkItemsAsync(
                        arr.ToArray(),
                        fields.ToArray(),
                        workItemQueryResult.AsOf).Result;

                    foreach (var workItem in workItems)
                    {
                        var smartyWorkItem =
                            new SmartyWorkItem { WorkItemId = workItem.Fields["System.Id"].ToString() };

                        if (workItem.Fields.ContainsKey("Microsoft.VSTS.Scheduling.RemainingWork"))
                        {
                            smartyWorkItem.RemainingHours = workItem.Fields["Microsoft.VSTS.Scheduling.RemainingWork"].ToString();
                        }

                        if (workItem.Fields.ContainsKey("Microsoft.VSTS.Scheduling.CompletedWork"))
                        {
                            smartyWorkItem.CompletedHours = workItem.Fields["Microsoft.VSTS.Scheduling.CompletedWork"].ToString();
                        }

                        smartyWorkItems.Add(smartyWorkItem);
                    }

                    return smartyWorkItems;
                }

                return null;
            }
        }
    }
}