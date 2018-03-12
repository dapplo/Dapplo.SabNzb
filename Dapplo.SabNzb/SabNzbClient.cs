//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.SabNzb
// 
//  Dapplo.SabNzb is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.SabNzb is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.SabNzb. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.HttpExtensions;
using Dapplo.HttpExtensions.JsonSimple;
using Dapplo.SabNzb.Entities;
using Dapplo.Utils.Extensions;

#endregion

namespace Dapplo.SabNzb
{
	public class SabNzbClient
	{
		private readonly string _apiKey;

		/// <summary>
		///     Store the specific HttpBehaviour, which contains a IHttpSettings and also some additional logic for making a
		///     HttpClient which works with Jira
		/// </summary>
		private readonly HttpBehaviour _behaviour;

		private string _password;

		private string _user;

		/// <summary>
		///     Create the SabNzbClient object, here the HttpClient is configured
		/// </summary>
		/// <param name="baseUri">Base URL, e.g. https://yoursabnzbserver</param>
		/// <param name="apiKey">API token</param>
		/// <param name="httpSettings">IHttpSettings or null for default</param>
		public SabNzbClient(Uri baseUri, string apiKey, IHttpSettings httpSettings = null)
		{
			if (baseUri == null)
			{
				throw new ArgumentNullException(nameof(baseUri));
			}

			_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

			SabNzbApiUri = baseUri.AppendSegments("api").ExtendQuery(new Dictionary<string, string>
			{
				{"output", "json"},
				{"apikey", apiKey}
			});

			_behaviour = new HttpBehaviour
			{
				HttpSettings = httpSettings ?? HttpExtensionsGlobals.HttpSettings,
				JsonSerializer = new SimpleJsonSerializer(),
				OnHttpRequestMessageCreated = httpMessage =>
				{
					if (!string.IsNullOrEmpty(_user) && _password != null)
					{
						httpMessage?.SetBasicAuthorization(_user, _password);
					}
					return httpMessage;
				}
			};
		}

		/// <summary>
		///     The base URI for your JIRA server
		/// </summary>
		public Uri SabNzbApiUri { get; }

		/// <summary>
		///     Set Basic Authentication for the current client
		/// </summary>
		/// <param name="user">username</param>
		/// <param name="password">password</param>
		public void SetBasicAuthentication(string user, string password)
		{
			_user = user;
			_password = password;
		}

		#region Misc

		/// <summary>
		///     Retrieve the version
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>version as string</returns>
		public async Task<string> GetVersionAsync(CancellationToken cancellationToken = default)
		{
			var versionUri = SabNzbApiUri.ExtendQuery("mode", "version");
			_behaviour.MakeCurrent();
			var container = await versionUri.GetAsAsync<VersionContainer>(cancellationToken);
			return container?.Version;
		}

		/// <summary>
		///     Retrieve the categories
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>List with categories</returns>
		public async Task<IList<string>> GetCategoriesAsync(CancellationToken cancellationToken = default)
		{
			var categoriesUri = SabNzbApiUri.ExtendQuery("mode", "get_cats");
			_behaviour.MakeCurrent();
			var container = await categoriesUri.GetAsAsync<CategoriesContainer>(cancellationToken);
			return container.Categories;
		}

		/// <summary>
		///     Retrieve the scripts
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>List with scripts</returns>
		public async Task<IList<string>> GetScriptsAsync(CancellationToken cancellationToken = default)
		{
			var scriptsUri = SabNzbApiUri.ExtendQuery("mode", "get_scripts");
			_behaviour.MakeCurrent();
			var container = await scriptsUri.GetAsAsync<ScriptsContainer>(cancellationToken);
			return container.Scripts;
		}

		/// <summary>
		///     Retrieve the warnings
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>List with warnings</returns>
		public async Task<IList<string>> GetWarningsAsync(CancellationToken cancellationToken = default)
		{
			var warningsUri = SabNzbApiUri.ExtendQuery("mode", "warnings");
			_behaviour.MakeCurrent();
			var container = await warningsUri.GetAsAsync<WarningsContainer>(cancellationToken);
			return container.Warnings;
		}

		/// <summary>
		///     Restart SabNzb
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task RestartAsync(CancellationToken cancellationToken = default)
		{
			var categoriesUri = SabNzbApiUri.ExtendQuery("mode", "restart");
			_behaviour.MakeCurrent();
			await categoriesUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Shutdown
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ShutdownAsync(CancellationToken cancellationToken = default)
		{
			var shutdownApi = SabNzbApiUri.ExtendQuery("mode", "shutdown");
			_behaviour.MakeCurrent();
			await shutdownApi.GetAsAsync<string>(cancellationToken);
		}

		#endregion

		#region Queue

		/// <summary>
		///     Retrieve the Queue
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>SabnzbDRoot</returns>
		public async Task<Queue> GetQueueAsync(CancellationToken cancellationToken = default)
		{
			var queueUri = SabNzbApiUri.ExtendQuery("mode", "queue");
			_behaviour.MakeCurrent();
			var root = await queueUri.GetAsAsync<SabnzbDRoot>(cancellationToken);
			return root?.QueueDetails;
		}

		/// <summary>
		///     Pause queue
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task PauseQueueAsync(CancellationToken cancellationToken = default)
		{
			var pauseUri = SabNzbApiUri.ExtendQuery("mode", "pause");
			_behaviour.MakeCurrent();
			await pauseUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Pause post processing
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task PausePostProcessingAsync(CancellationToken cancellationToken = default)
		{
			var pausePostProcessingUri = SabNzbApiUri.ExtendQuery("mode", "pause_pp");
			_behaviour.MakeCurrent();
			await pausePostProcessingUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Resume post processing
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ResumePostProcessingAsync(CancellationToken cancellationToken = default)
		{
			var resumePostProcessingUri = SabNzbApiUri.ExtendQuery("mode", "resume_pp");
			_behaviour.MakeCurrent();
			await resumePostProcessingUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Resume queue
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ResumeQueueAsync(CancellationToken cancellationToken = default)
		{
			var resumeUri = SabNzbApiUri.ExtendQuery("mode", "resume");
			_behaviour.MakeCurrent();
			await resumeUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Change the action which is performed if the queue is empty
		/// </summary>
		/// <param name="endOfQueueAction">EndOfQueueActions</param>
		/// <param name="script">If EndOfQueueActions.Script is specified, also specify the script</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ChangeEndOfQueueActionAsync(EndOfQueueActions endOfQueueAction, string script = null, CancellationToken cancellationToken = default)
		{
			var value = endOfQueueAction.EnumValueOf();
			// In case of script, append this.
			if (endOfQueueAction == EndOfQueueActions.Script)
			{
				if (script == null)
				{
					throw new ArgumentNullException(nameof(script));
				}
				value = value + script;
			}
			var completeActionUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "queue"},
				{"name", "change_complete_action"},
				{"value", value}
			});
			_behaviour.MakeCurrent();
			await completeActionUri.GetAsAsync<string>(cancellationToken);
		}

		#endregion

		#region Modify QueueSlot

		/// <summary>
		///     Delete the supplied item(s) from the queue, use "all" for all
		/// </summary>
		/// <param name="items">an ienumerable with item ids</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task DeleteFromQueueAsync(IEnumerable<string> items, CancellationToken cancellationToken = default)
		{
			var deleteUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "queue"},
				{"name", "delete"},
				{"value", string.Join(",", items)}
			});

			_behaviour.MakeCurrent();
			await deleteUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Change the script for a download (slot)
		/// </summary>
		/// <param name="id">string with the id of the slot</param>
		/// <param name="script">name of the script</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ChangeScriptAsync(string id, string script, CancellationToken cancellationToken = default)
		{
			// api?mode=change_script&value=SABnzbd_nzo_zt2syz&value2=examplescript.cmd
			var changeScriptUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "change_script"},
				{"value", id},
				{"value2", script}
			});
			_behaviour.MakeCurrent();
			await changeScriptUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Change the category for a download (slot)
		/// </summary>
		/// <param name="id">string with the id of the slot</param>
		/// <param name="category">category</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ChangeCategoryAsync(string id, string category, CancellationToken cancellationToken = default)
		{
			// api?mode=change_cat&value=SABnzbd_nzo_zt2syz&value2=Example
			var changeCategoryUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "change_cat"},
				{"value", id},
				{"value2", category}
			});
			_behaviour.MakeCurrent();
			await changeCategoryUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Change the priority for a download (slot)
		/// </summary>
		/// <param name="id">string with the id of the slot</param>
		/// <param name="priority">priority</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ChangePriorityAsync(string id, Priority priority, CancellationToken cancellationToken = default)
		{
			// api?mode=queue&name=priority&value=SABnzbd_nzo_zt2syz&value2=0
			var changePriorityUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "queue"},
				{"name", "priority"},
				{"value", id},
				{"value2", ((int) priority).ToString()}
			});
			_behaviour.MakeCurrent();
			await changePriorityUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Change the post processing for a download (slot)
		/// </summary>
		/// <param name="id">string with the id of the slot</param>
		/// <param name="postProcessing">PostProcessing</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ChangePostProcessingAsync(string id, PostProcessing postProcessing, CancellationToken cancellationToken = default)
		{
			//  api?mode=change_opts&value=SABnzbd_nzo_zt2syz&value2=0
			var changePostProcessingUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "change_opts"},
				{"value", id},
				{"value2", ((int) postProcessing).ToString()}
			});
			_behaviour.MakeCurrent();
			await changePostProcessingUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Change the name for a download (slot)
		/// </summary>
		/// <param name="id">string with the id of the slot</param>
		/// <param name="name">New name</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ChangeNameAsync(string id, string name, CancellationToken cancellationToken = default)
		{
			// api?mode=queue&name=rename&value=SABnzbd_nzo_zt2syz&value2=THENEWNAME
			var changeNameUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "queue"},
				{"name", "rename"},
				{"value", id},
				{"value2", name}
			});
			_behaviour.MakeCurrent();
			await changeNameUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Pause a download (slot)
		/// </summary>
		/// <param name="id">string with the id of the slot</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task PauseAsync(string id, CancellationToken cancellationToken = default)
		{
			// api?mode=queue&name=pause&value=SABnzbd_nzo_zt2syz
			var pauseUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "queue"},
				{"name", "pause"},
				{"value", id}
			});
			_behaviour.MakeCurrent();
			await pauseUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Resume a download (slot)
		/// </summary>
		/// <param name="id">string with the id of the slot</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task ResumeAsync(string id, CancellationToken cancellationToken = default)
		{
			//  api?mode=queue&name=resume&value=SABnzbd_nzo_zt2syz
			var pauseUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "queue"},
				{"name", "resume"},
				{"value", id}
			});
			_behaviour.MakeCurrent();
			await pauseUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Retrieve the details of a slot
		/// </summary>
		/// <param name="id">Id of the slot</param>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>QueueSlot</returns>
		public async Task<QueueSlot> RetrieveDetailsAsync(string id, CancellationToken cancellationToken = default)
		{
			// api?mode=get_files&output=xml&value=SABnzbd_nzo_zt2syz
			var slotUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "get_files"},
				{"value", id}
			});
			_behaviour.MakeCurrent();
			return await slotUri.GetAsAsync<QueueSlot>(cancellationToken);
		}

		/// <summary>
		///     Add external url, pointing to a NZB file, sabnzb will download this and start downloading.
		/// </summary>
		/// <param name="externalUri">Url to the sabnzb file</param>
		/// <param name="category">Category</param>
		/// <param name="priority">Priority</param>
		/// <param name="name">Nice name</param>
		/// <param name="script">script to execute, </param>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>NZO Id</returns>
		public async Task<string> AddAsync(Uri externalUri, string category = null, Priority priority = Priority.Default, string name = null, string script = null, CancellationToken cancellationToken = default)
		{
			var addUri = SabNzbApiUri.ExtendQuery("mode", "addurl");
			addUri = addUri.ExtendQuery("name", externalUri.AbsoluteUri);
			addUri = addUri.ExtendQuery("priority", priority);
			if (!string.IsNullOrEmpty(category))
			{
				addUri = addUri.ExtendQuery("cat", category);
			}
			if (!string.IsNullOrEmpty(name))
			{
				addUri = addUri.ExtendQuery("nzbname", category);
			}
			if (!string.IsNullOrEmpty(script))
			{
				addUri = addUri.ExtendQuery("script", script);
			}
			_behaviour.MakeCurrent();
			return await addUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Add file, sabnzb will download this and start downloading.
		/// </summary>
		/// <param name="filename">Url to the sabnzb file</param>
		/// <param name="filestream">Stream for the file content</param>
		/// <param name="nzbName">Nice name</param>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>NZO Id</returns>
		public async Task<string> AddAsync(string filename, Stream filestream, string nzbName = null, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentNullException(nameof(filename));
			}
			if (filestream == null)
			{
				throw new ArgumentNullException(nameof(filestream));
			}

			// Prepare upload data
			var nzbUpload = new NzbUpload
			{
				NzbContent = filestream,
				NzbFileName = filename,
				NzbName = nzbName,
				ApiKey = _apiKey
			};
			// Build Uri, this should no longer have query parameters
			var builder = new UriBuilder(SabNzbApiUri)
			{
				Query = ""
			};
			var addUri = builder.Uri;
			_behaviour.MakeCurrent();
			var response = await addUri.PostAsync<NzbUploadResponse>(nzbUpload, cancellationToken);

			// Check status, and if it's true get the ID
			if (response.Status)
			{
				return response.NzoIds.FirstOrDefault();
			}
			// Throw an exception, as there is not correct status from SabNZB
			throw new InvalidOperationException("Couldn't upload nzb file to start download.");
		}

		#endregion

		#region History

		/// <summary>
		///     Retrieve the History
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>SabnzbDRoot</returns>
		public async Task<History> GetHistoryAsync(CancellationToken cancellationToken = default)
		{
			var queueUri = SabNzbApiUri.ExtendQuery("mode", "history");
			_behaviour.MakeCurrent();
			var root = await queueUri.GetAsAsync<SabnzbDRoot>(cancellationToken);
			return root.HistoryDetails;
		}

		/// <summary>
		///     Delete the supplied item(s) from the history, use "all" for all or "failed" for all failed
		/// </summary>
		/// <param name="items">an ienumerable with item ids</param>
		/// <param name="failedOnly"></param>
		/// <param name="deleteFiles"></param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task DeleteFromHistoryAsync(IEnumerable<string> items, bool failedOnly = false, bool deleteFiles = false, CancellationToken cancellationToken = default)
		{
			// failed_only, when 1, delete only failed jobs
			// del_files when 1, all files will be deleted too (only for failed jobs).
			var deleteUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "history"},
				{"name", "delete"},
				{"failed_only", failedOnly ? "1" : "0"},
				{"del_files", deleteFiles ? "1" : "0"},
				{"value", string.Join(",", items)}
			});

			_behaviour.MakeCurrent();
			await deleteUri.GetAsAsync<string>(cancellationToken);
		}

		/// <summary>
		///     Retry download/unpack of a failed item
		/// </summary>
		/// <param name="id">id of item to retry</param>
		/// <param name="cancellationToken">CancellationToken</param>
		public async Task RetryAsync(string id, CancellationToken cancellationToken = default)
		{
			//  api?mode=retry&value=SABnzbd_nzo_zt2syz
			var retryUri = SabNzbApiUri.ExtendQuery(new Dictionary<string, string>
			{
				{"mode", "retry"},
				{"value", id}
			});

			_behaviour.MakeCurrent();
			await retryUri.GetAsAsync<string>(cancellationToken);
		}

		#endregion
	}
}