using MySqlConnector;
using Dapper;
using FinalProject.Models;
using System.Net;
using System.Transactions;

namespace FinalProject.Database
{
	public class JobTaskDBAccessor : DBAccessor
	{
		public override void CreateTable()
		{
			try
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
			}
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:" + ex.Message);
			}
			finally
			{
				connection.Close();
			}
		}

		public Task Insert(JobTask jobTask)
		{
			try
			{
				connection.Open();

				string sql = "INSERT INTO JobTask (TaskId, TaskName, StartDate, EndDate, Status, ProjectId) VALUES (@TaskId, @TaskName, @StartDate, @EndDate, @Status, @ProjectId)";

				connection.Execute(sql, jobTask);
			}
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
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
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
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
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
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
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
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
			catch (MySqlException ex)
			{
				Console.WriteLine("MySQL Error:" + ex.Message);
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
		public async Task Delete(JobTask jobTask)
		{
			await connection.OpenAsync();
			using (var transaction = await connection.BeginTransactionAsync())
			{
				try
				{
					// Delete from JobTaskMemberLink table
					string linkSql = "DELETE FROM JobTaskMemberLink WHERE TaskId = @TaskId";
					await connection.ExecuteAsync(linkSql, jobTask, transaction: transaction);

					// Delete from JobTask table
					string sql = "DELETE FROM JobTask WHERE TaskId = @TaskId";
					await connection.ExecuteAsync(sql, jobTask, transaction: transaction);

					await transaction.CommitAsync();

				}
				catch (MySqlException ex)
				{
					Console.WriteLine("MySQL Error:" + ex.Message);
					transaction.Rollback();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error:" + ex.Message);
					transaction.Rollback();
				}
				finally
				{
					connection.Close();
				}
			}
			//return Task.CompletedTask;
		}

	}
}
