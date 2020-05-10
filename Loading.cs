using System.Text;
using UnityEngine;
using ICities;
using ColossalFramework;


namespace RemoveSickness
{
    public class Loading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            // Only run on loaded games (existing saves).
            if (mode != LoadMode.LoadGame)
            {
                return;
            }

            base.OnLevelLoaded(mode);

            // Get CitizenManager instance.
            CitizenManager citizenManager = Singleton<CitizenManager>.instance;

            StringBuilder logMessage = new StringBuilder("Remove Sickness mod:\r\n");

            // Iterate though each citizen.
            for (int i = 0; i < citizenManager.m_citizens.m_buffer.Length; i ++)
            {
                // Use zero age as a quick filter for non-existing citizens.
                if (citizenManager.m_citizens.m_buffer[i].m_age > 0)
                {
                    // Add citizen data to logging buffer.
                    logMessage.Append("Removing sickness from citizen ");
                    logMessage.Append(i);
                    logMessage.Append(" (age ");
                    logMessage.Append(citizenManager.m_citizens.m_buffer[i].m_age);
                    logMessage.Append(") with health status: ");
                    logMessage.Append(citizenManager.m_citizens.m_buffer[i].Sick ? "sick" : "healthy");
                    logMessage.Append(" and health ");
                    logMessage.Append(citizenManager.m_citizens.m_buffer[i].m_health);
                    logMessage.AppendLine(".");

                    // Heal!
                    citizenManager.m_citizens.m_buffer[i].Sick = false;

                    // Also make sure general health level is at least 51 (ResidentAI.UpdateHealth() threshold for setting bad health).
                    if (citizenManager.m_citizens.m_buffer[i].m_health < 51)
                    {
                        citizenManager.m_citizens.m_buffer[i].m_health = 51;
                        citizenManager.m_citizens.m_buffer[i].BadHealth = 0;
                    }
                }
            }

            // Log the buffer.
            Debug.Log(logMessage);
        }
    }
}