export interface SystemEventListItem {
  systemEventId: number;
  correlationId: string;
  eventType: string;
  entityType: string;
  entityId?: number;
  eventDataJson?: string;
  scenarioName?: string;
  createdUtc: string;
  createdBy: string;
}