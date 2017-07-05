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
using Google.Api.Ads.Dfp.v201611;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201611 {
  /// <summary>
  /// This code example creates new proposals. To determine which proposals exist,
  /// run GetAllProposals.cs.
  /// </summary>
  public class CreateProposals : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example creates new proposals. To determine which proposals exist, " +
            "run GetAllProposals.cs.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      CreateProposals codeExample = new CreateProposals();
      Console.WriteLine(codeExample.Description);
      codeExample.Run(new DfpUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser user) {

      // [START get_proposal_service] MOE:strip_line
      // Get the ProposalService.
      ProposalService proposalService =
          (ProposalService) user.GetService(DfpService.v201611.ProposalService);
      // [END get_proposal_service] MOE:strip_line

      // Get the NetworkService.
      NetworkService networkService =
          (NetworkService) user.GetService(DfpService.v201611.NetworkService);

      // Set the advertiser, salesperson, and trafficker to assign to each
      // order.
      long advertiserId = long.Parse(_T("INSERT_ADVERTISER_ID_HERE"));
      long primarySalespersonId = long.Parse(_T("INSERT_PRIMARY_SALESPERSON_ID_HERE"));
      long secondarySalespersonId = long.Parse(_T("INSERT_SECONDARY_SALESPERSON_ID_HERE"));
      long primaryTraffickerId = long.Parse(_T("INSERT_PRIMARY_TRAFFICKER_ID_HERE"));
      long secondaryTraffickerId = long.Parse(_T("INSERT_SECONDARY_TRAFFICKER_ID_HERE"));

      // [START create_proposal_local] MOE:strip_line
      // Create a proposal.
      Proposal proposal = new Proposal();
      proposal.name = "Proposal #" + new Random().Next(int.MaxValue);
      // [END create_proposal_local] MOE:strip_line

      // [START add_proposal_company_associations] MOE:strip_line
      // Create a proposal company association.
      ProposalCompanyAssociation proposalCompanyAssociation = new ProposalCompanyAssociation();
      proposalCompanyAssociation.companyId = advertiserId;
      proposalCompanyAssociation.type = ProposalCompanyAssociationType.ADVERTISER;
      proposal.advertiser = proposalCompanyAssociation;
      // [END add_proposal_company_associations] MOE:strip_line

      // [START add_salespeople] MOE:strip_line
      // Create salesperson splits for the primary salesperson and secondary salespeople.
      SalespersonSplit primarySalesperson = new SalespersonSplit();
      primarySalesperson.userId = primarySalespersonId;
      primarySalesperson.split = 75000;
      proposal.primarySalesperson = primarySalesperson;

      SalespersonSplit secondarySalesperson = new SalespersonSplit();
      secondarySalesperson.userId = secondarySalespersonId;
      secondarySalesperson.split = 25000;
      proposal.secondarySalespeople = new SalespersonSplit[] {secondarySalesperson};
      // [END add_salespeople] MOE:strip_line

      // [START set_probability_to_close] MOE:strip_line
      // Set the probability to close to 100%.
      proposal.probabilityOfClose = 100000L;
      // [END set_probability_to_close] MOE:strip_line

      // [START set_primary_trafficker] MOE:strip_line
      // Set the primary trafficker on the proposal for when it becomes an order.
      proposal.primaryTraffickerId = primaryTraffickerId;
      // [END set_primary_trafficker] MOE:strip_line

      // [START set_budget] MOE:strip_line
      // Create a budget for the proposal worth 100 in the network local currency.
      Money budget = new Money();
      budget.microAmount = 100000000L;
      budget.currencyCode = networkService.getCurrentNetwork().currencyCode;
      proposal.budget = budget;
      // [END set_budget] MOE:strip_line

      // [START set_billing_info] MOE:strip_line
      proposal.billingCap = BillingCap.CAPPED_CUMULATIVE;
      proposal.billingSource = BillingSource.DFP_VOLUME;
      // [END set_billing_info] MOE:strip_line

      try {
        // [START create_proposal_server] MOE:strip_line
        // Create the proposal on the server.
        Proposal[] proposals = proposalService.createProposals(new Proposal[] {proposal});
        // [END create_proposal_server] MOE:strip_line

        foreach (Proposal createdProposal in proposals) {
          Console.WriteLine("A proposal with ID \"{0}\" and name \"{1}\" was created.",
              createdProposal.id, createdProposal.name);
        }
      } catch (Exception e) {
          Console.WriteLine("Failed to create proposals. Exception says \"{0}\"",
                            e.Message);
      }
    }
  }
}
