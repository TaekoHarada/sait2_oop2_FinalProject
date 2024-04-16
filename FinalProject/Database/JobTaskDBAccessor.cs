using MySqlConnector;
using Dapper;
using FinalProject.Models;
using System.Net;

namespace FinalProject.Database
{
	public class JobTaskDBAccessor : DBAccessor
	{
		public override void CreateTable()
		{
			connection.Open();

			var sql = @"CREATE TABLE IF NOT EXISTS jobtask (
                TaskId VARCHAR(36) PRIMARY KEY,
                TaskName VARCHAR(255) NOT NULL,
                StartDate DATETIME,
                EndDate DATETIME,
                Status INT(1) NOT NULL,
                ProjectId VARCHAR(36) NOT NULL,
				    CONSTRAINT T_ProjectId_FK FOREIGN KEY (ProjectId) REFERENCES Project(ProjectId)
            )";

			connection.Execute(sql);

			connection.Close();

		}

		public Task Insert(JobTask jobTask)
		{
			try
			{
				connection.Open();

				string sql = "INSERT INTO JobTask (TaskId, TaskName, StartDate, EndDate, Status, ProjectId) VALUES (@TaskId, @TaskName, @StartDate, @EndDate, @Status, @ProjectId)";

				connection.Execute(sql, jobTask);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}
			return Task.CompletedTask;
		}

		public List<JobTask> SelectAll()
		{
			List<JobTask> jobTasks = new List<JobTask>();

			try
			{
				connection.Open();

				string sql = "SELECT * FROM JobTask";

				jobTasks = connection.Query<JobTask>(sql).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}

			return jobTasks;
		}

		public List<JobTask> SelectByProject(string projectId)
		{
			List<JobTask> jobTasks = new List<JobTask>();

			try
			{
				connection.Open();

				string sql = "SELECT * FROM JobTask WHERE ProjectId=@ProjectId";

				jobTasks = connection.Query<JobTask>(sql, new { ProjectId = projectId }).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}

			return jobTasks;
		}

		public JobTask SelectById(string taskId)
		{
			JobTask jobTask = new JobTask();
			try
			{
				connection.Open();

				string sql = $"SELECT * FROM JobTask WHERE taskId='{taskId}'";

				jobTask = connection.QuerySingle<JobTask>(sql);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}

			return jobTask;
		}

		public Task Update(JobTask jobTask)
		{
			try
			{
				connection.Open();

				string sql = "UPDATE JobTask SET TaskId=@TaskId, TaskName=@TaskName, StartDate=@StartDate, EndDate=@EndDate, Status=@Status, ProjectId=@ProjectId WHERE TaskId=@TaskId";

				connection.Execute(sql, jobTask);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}

			return Task.CompletedTask;
		}
		public Task Delete(JobTask jobTask)
		{
			try
			{
				connection.Open();

				string sql = "DELETE FROM JobTask WHERE TaskId = @TaskId";

				connection.Execute(sql, jobTask);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}
			return Task.CompletedTask;
		}




	}
}
