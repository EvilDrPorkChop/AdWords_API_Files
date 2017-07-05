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
using Google.Api.Ads.Dfp.Util.v201611;
using Google.Api.Ads.Dfp.v201611;
using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201611 {
  /// <summary>
  /// This example gets all content metadata key hierarchies.
  /// </summary>
  public class GetAllContentMetadataKeyHierarchies : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all content metadata key hierarchies.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetAllContentMetadataKeyHierarchies codeExample = new GetAllContentMetadataKeyHierarchies();
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new DfpUser());
      } catch (Exception e) {
        Console.WriteLine("Failed to get content metadata key hierarchies. Exception says \"{0}\"",
            e.Message);
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser dfpUser) {
      ContentMetadataKeyHierarchyService contentMetadataKeyHierarchyService =
          (ContentMetadataKeyHierarchyService) dfpUser.GetService(DfpService.v201611.ContentMetadataKeyHierarchyService);

      // Create a statement to select content metadata key hierarchies.
      int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
      StatementBuilder statementBuilder = new StatementBuilder()
          .OrderBy("id ASC")
          .Limit(pageSize);

      // Retrieve a small amount of content metadata key hierarchies at a time, paging through until all
      // content metadata key hierarchies have been retrieved.
      int totalResultSetSize = 0;
      do {
        ContentMetadataKeyHierarchyPage page = contentMetadataKeyHierarchyService.getContentMetadataKeyHierarchiesByStatement(
            statementBuilder.ToStatement());

        // Print out some information for each content metadata key hierarchy.
        if (page.results != null) {
          totalResultSetSize = page.totalResultSetSize;
          int i = page.startIndex;
          foreach (ContentMetadataKeyHierarchy contentMetadataKeyHierarchy in page.results) {
            Console.WriteLine(
                "{0}) Content metadata key hierarchy with ID {1} and name \"{2}\" was found.",
                i++,
                contentMetadataKeyHierarchy.id,
                contentMetadataKeyHierarchy.name
            );
          }
        }

        statementBuilder.IncreaseOffsetBy(pageSize);
      } while (statementBuilder.GetOffset() < totalResultSetSize);

      Console.WriteLine("Number of results found: {0}", totalResultSetSize);
    }
  }
}
