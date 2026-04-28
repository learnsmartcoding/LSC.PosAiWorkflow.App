export interface ApprovalRequestListItem {
  approvalRequestId: number;
  aiDecisionId: number;
  productId: number;
  productName: string;
  approvalType: string;
  status: string;
  requestedBy: string;
  requestedUtc: string;
  scenarioName?: string;
}

export interface ApprovalActionRequest {
  reviewedBy: string;
  reviewComments?: string;
}