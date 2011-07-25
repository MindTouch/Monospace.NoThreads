using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindTouch.Dream;
using MindTouch.Tasking;
using MindTouch.Xml;
using NUnit.Framework;

namespace Monospace.NoThreads.State {
    [TestFixture]
    public class AsyncMethodExample {

        [Test]
        public void Calling_Task_async_method() {

            // this will always return immediately and never blocks
            var task = GetRecentTweets("#monospace");

            // this should (but doesn't have to) return false
            Console.WriteLine("finished: {0}", task.IsCompleted);

            // blocking for async call to complete && iterate over result
            foreach(var tweet in task.Result) {
                Console.WriteLine("{0}: {1}", tweet.Who, tweet.What);
            }
        }

        public Task<IEnumerable<Tweet>> GetRecentTweets(string search) {

            // create a handle that can have a result set on it
            var tcs = new TaskCompletionSource<IEnumerable<Tweet>>();

            // asynchronously make and http request
            Plug.New("http://search.twitter.com").At("search.atom").With("q", search)
                .Get(new Result<XDoc>())
                .WhenDone(r => tcs.SetResult(r.Value.ToTweets()));

            // immediately return the task that can be used to await the call to complete
            return tcs.Task;
        }

        [Test]
        public void Calling_Result_async_method() {

            // this will always return immediately and never blocks
            var result = GetRecentTweets("#monospace", new Result<IEnumerable<Tweet>>());

            // this should (but doesn't have to) return false
            Console.WriteLine("finished: {0}", result.HasFinished);

            // blocking for async call to complete && iterate over result
            foreach(var tweet in result.Wait()) {
                Console.WriteLine("{0}: {1}", tweet.Who, tweet.What);
            }
        }

        public Result<IEnumerable<Tweet>> GetRecentTweets(string search, Result<IEnumerable<Tweet>> result) {

            // asynchronously make and http request
            Plug.New("http://search.twitter.com").At("search.atom").With("q", search)
                .Get(new Result<XDoc>())
                .WhenDone(r => result.Return(r.Value.ToTweets()));

            // immediately return the result that can be used to await the call to complete
            return result;
        }
    }

    public static class TweetEx {
        public static IEnumerable<Tweet> ToTweets(this XDoc doc) {
            doc = doc.UsePrefix("atom", "http://www.w3.org/2005/Atom");
            return from entry in doc["atom:entry"]
                   select new Tweet {
                       Who = entry["atom:author/atom:name"].AsText,
                       What = entry["atom:title"].AsText
                   };
        }
    }

    public class Tweet {
        public string Who;
        public string What;
    }
}