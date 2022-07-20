using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TribalWarsBot
{
    internal class ScavengeModule
    {
        // code remnant - private readonly List<Task> scavengeTasks = new();
        private readonly List<JobScavenge> jobScavenges = new();
        public ScavengeModule()
        {
            Refresh();
        }
        /** Refresh existing scavenges to react to changes made in ScavengeConfig.yaml file */
        public void Refresh()
        {
            List<ScavengeVillage>? scavengeVillages = ScavengeVillages.LoadScavengeConfig().Villages;
            if (scavengeVillages == null) return;
            // -----------------------------------------------------------
            // Firstly remove existing scavenge villages, if not specified
            foreach(var existScavengeJob in this.jobScavenges)
            {
                string existName = existScavengeJob.village.Name;
                bool found = false;
                foreach(var newVillage in scavengeVillages)
                {
                    if (newVillage.Name == existName)
                    {
                        found = true;
                        // Secondly update currently running tasks
                        UpdateScavengeVillage(existScavengeJob, newVillage);
                    }
                }
                // Remove if found == false
                if (!found) RemoveScavengeVillage(existScavengeJob);
            }
            // -----------------------------------------------------------
            // Create new scavenge tasks
            foreach (var newVillage in scavengeVillages)
            {
                bool found = false;
                foreach(var existScavengeJob in this.jobScavenges)
                {
                    if(existScavengeJob.village.Name == newVillage.Name)
                    {
                        found = true;
                    }
                }
                // Create if found == false
                if (!found) CreateScavengeVillage(newVillage);
            }
        }
        /** Updates new village with updated preferences to existing task/job */
        public static void UpdateScavengeVillage(JobScavenge job, ScavengeVillage village)
        {
            job.village = village;
        }
        /** Adds new village and subsequent task/job */
        public void CreateScavengeVillage(ScavengeVillage village)
        {
            JobScavenge job = new(village);
            jobScavenges.Add(job);
            Task t = new(job.Start);
            //code remnant - scavengeTasks.Add(t);
            t.Start();
        }
        /** Removes the job that has been deleted from config,
         ** subsequent task should end itself after Start method is not allowed to run */
        public void RemoveScavengeVillage(JobScavenge job)
        {
            job.Run = false;
            jobScavenges.Remove(job);
        }
    }
}
