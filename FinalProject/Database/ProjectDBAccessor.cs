using Dapper;
using FinalProject.Models;
using MySqlConnector;
using System.Net;

namespace FinalProject.Database
{
	public class ProjectDBAccessor : DBAccessor
	{
		/// <summary>
		/// Create project table
		/// </summary>
		public override void CreateTable()
		{
			connection.Open();

			var sql = @"CREATE TABLE IF NOT EXISTS project (
                ProjectId VARCHAR(36) PRIMARY KEY,
                ProjectName VARCHAR(255) NOT NULL
            )";

			connection.Execute(sql);

			connection.Close();

		}

		/// <summary>
		/// Add a new project
		/// </summary>
		/// <param name="project"></param>
		public Task Insert(Project project)
		{
			try
			{
				connection.Open();

				string sql = "INSERT INTO project (ProjectId, ProjectName) VALUES (@ProjectId, @ProjectName)";

				connection.Execute(sql, project);
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

		/// <summary>
		/// Get all projects
		/// </summary>
		/// <returns> List<Project> </returns>
		public List<Project> SelectAll()
		{
			List<Project> projects = new List<Project>();

			try
			{
				connection.Open();

				string sql = "SELECT * FROM project";

				projects = connection.Query<Project>(sql).ToList();
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

			return projects;
		}

		/// <summary>
		/// Get a project by projectId
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns></returns>
		public Project SelectById(string projectId)
		{
			Project project = new Project();
			try
			{
				connection.Open();

				string sql = $"SELECT * FROM Project WHERE projectId='{projectId}'";

				project = connection.QuerySingle<Project>(sql);
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

			return project;
		}

		public Task Update(Project project)
		{
			try
			{
				connection.Open();

				string sql = "UPDATE Project SET ProjectId=@ProjectId, ProjectName=@ProjectName WHERE ProjectId=@ProjectId";

				connection.Execute(sql, project);
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
		public async Task Delete(Project project)
		{
			await connection.OpenAsync();
			using (var transaction = await connection.BeginTransactionAsync())
			{
				try
				{
					string linkSql = "DELETE FROM JobTaskMemberLink WHERE TaskId in (select TaskId from JobTask  WHERE ProjectId = @ProjectId)";
					await connection.ExecuteAsync(linkSql, project, transaction: transaction);

					string jobTaskSql = "DELETE FROM JobTask WHERE ProjectId = @ProjectId";
					await connection.ExecuteAsync(jobTaskSql, project, transaction: transaction);

					string projectSql = "DELETE FROM project WHERE ProjectId = @ProjectId";
					await connection.ExecuteAsync(projectSql, project, transaction: transaction);

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
			// return Task.CompletedTask;
		}


	}
}
