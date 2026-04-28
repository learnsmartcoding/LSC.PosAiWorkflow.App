export interface AiDecisionListItem {
  aiDecisionId: number;
  productId: number;
  productName: string;
  storeCode: string;
  correlationId: string;
  decisionType: string;
  recommendedAction: string;
  confidenceScore?: number;
  riskLevel?: string;
  decisionStatus: string;
  suggestedQuantity?: number;
  estimatedTotalCost?: number;
  requiresApproval?: boolean;
  scenarioName?: string;
  createdUtc: string;
}

export interface AiDecisionDetail {
  aiDecisionId: number;
  productId: number;
  productName: string;
  storeCode: string;
  correlationId: string;
  decisionType: string;
  entityType: string;
  entityId: number;
  modelName: string;
  promptVersion: string;
  inputSummaryJson: string;
  decisionJson: string;
  reasoning: string;
  recommendedAction: string;
  confidenceScore?: number;
  riskLevel?: string;
  decisionStatus: string;
  suggestedQuantity?: number;
  estimatedUnitCost?: number;
  estimatedTotalCost?: number;
  requiresApproval?: boolean;
  scenarioName?: string;
  createdUtc: string;
  approvedUtc?: string;
  rejectedUtc?: string;
  executedUtc?: string;
}