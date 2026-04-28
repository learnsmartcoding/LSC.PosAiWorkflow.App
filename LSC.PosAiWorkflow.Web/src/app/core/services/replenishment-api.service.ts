import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AiDecisionDetail, AiDecisionListItem } from '../models/ai-decision.models';
import { ApprovalActionRequest, ApprovalRequestListItem } from '../models/approval.models';
import { SystemEventListItem } from '../models/system-event.models';

@Injectable({
  providedIn: 'root'
})
export class ReplenishmentApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  getAiDecisions(take = 50): Observable<AiDecisionListItem[]> {
    return this.http.get<AiDecisionListItem[]>(`${this.baseUrl}/AiDecisions?take=${take}`);
  }

  getAiDecisionById(aiDecisionId: number): Observable<AiDecisionDetail> {
    return this.http.get<AiDecisionDetail>(`${this.baseUrl}/AiDecisions/${aiDecisionId}`);
  }

  getPendingApprovals(): Observable<ApprovalRequestListItem[]> {
    return this.http.get<ApprovalRequestListItem[]>(`${this.baseUrl}/Approvals/pending`);
  }

  getRecentApprovals(take = 50): Observable<ApprovalRequestListItem[]> {
    return this.http.get<ApprovalRequestListItem[]>(`${this.baseUrl}/Approvals?take=${take}`);
  }

  approveApprovalRequest(approvalRequestId: number, request: ApprovalActionRequest): Observable<unknown> {
    return this.http.post(`${this.baseUrl}/Approvals/${approvalRequestId}/approve`, request);
  }

  rejectApprovalRequest(approvalRequestId: number, request: ApprovalActionRequest): Observable<unknown> {
    return this.http.post(`${this.baseUrl}/Approvals/${approvalRequestId}/reject`, request);
  }

  getSystemEventsByCorrelationId(correlationId: string): Observable<SystemEventListItem[]> {
    return this.http.get<SystemEventListItem[]>(`${this.baseUrl}/SystemEvents/by-correlation/${correlationId}`);
  }

  getRecentSystemEvents(take = 100): Observable<SystemEventListItem[]> {
    return this.http.get<SystemEventListItem[]>(`${this.baseUrl}/SystemEvents?take=${take}`);
  }
}