// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201702;
using Google.Api.Ads.Dfp.v201702;
using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201702 {
  /// <summary>
  /// This example gets all highly requested suggested ad units.
  /// </summary>
  public class GetHighlyRequestedSuggestedAdUnits : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all highly requested suggested ad units.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetHighlyRequestedSuggestedAdUnits codeExample = new GetHighlyRequestedSuggestedAdUnits();
      long numRequests = long.Parse("INSERT_NUM_REQUESTS_HERE");
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new DfpUser(), numRequests);
      } catch (Exception e) {
        Console.WriteLine("Failed to get suggested ad units. Exception says \"{0}\"",
            e.Message);
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser dfpUser, long numRequests) {
      SuggestedAdUnitService suggestedAdUnitService =
          (SuggestedAdUnitService) dfpUser.GetService(DfpService.v201702.SuggestedAdUnitService);

      // Create a statement to select suggested ad units.
      int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("numRequests >= :numRequests")
          .OrderBy("id ASC")
          .Limit(pageSize)
          .AddValue("numRequests", numRequests);

      // Retrieve a small amount of suggested ad units at a time, paging through until all
      // suggested ad units have been retrieved.
      int totalResultSetSize = 0;
      do {
        SuggestedAdUnitPage page = suggestedAdUnitService.getSuggestedAdUnitsByStatement(
            statementBuilder.ToStatement());

        // Print out some information for each suggested ad unit.
        if (page.results != null) {
          totalResultSetSize = page.totalResultSetSize;
          int i = page.startIndex;
          foreach (SuggestedAdUnit suggestedAdUnit in page.results) {
            Console.WriteLine(
                "{0}) Suggested ad unit with ID \"{1}\" and num requests {2} was found.",
                i++,
                suggestedAdUnit.id,
                suggestedAdUnit.numRequests
            );
          }
        }

        statementBuilder.IncreaseOffsetBy(pageSize);
      } while (statementBuilder.GetOffset() < totalResultSetSize);

      Console.WriteLine("Number of results found: {0}", totalResultSetSize);
    }
  }
}
