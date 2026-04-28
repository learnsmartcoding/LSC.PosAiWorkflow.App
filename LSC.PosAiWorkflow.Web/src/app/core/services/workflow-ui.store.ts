import { Injectable, computed, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AiDecisionDetail, AiDecisionListItem } from '../models/ai-decision.models';
import { ApprovalRequestListItem } from '../models/approval.models';
import { SystemEventListItem } from '../models/system-event.models';
import { ReplenishmentApiService } from './replenishment-api.service';

@Injectable({
  providedIn: 'root'
})
export class WorkflowUiStore {
  private readonly api = inject(ReplenishmentApiService);

  private readonly decisionsState = signal({
    isLoading: false,
    errorMessage: '',
    data: [] as AiDecisionListItem[]
  });

  private readonly decisionDetailState = signal({
    isLoading: false,
    errorMessage: '',
    data: null as AiDecisionDetail | null
  });

  private readonly approvalsState = signal({
    isLoading: false,
    errorMessage: '',
    data: [] as ApprovalRequestListItem[]
  });

  private readonly traceState = signal({
    isLoading: false,
    errorMessage: '',
    data: [] as SystemEventListItem[]
  });

  readonly decisions = computed(() => this.decisionsState().data);
  readonly decisionsLoading = computed(() => this.decisionsState().isLoading);
  readonly decisionsError = computed(() => this.decisionsState().errorMessage);

  readonly decisionDetail = computed(() => this.decisionDetailState().data);
  readonly decisionDetailLoading = computed(() => this.decisionDetailState().isLoading);
  readonly decisionDetailError = computed(() => this.decisionDetailState().errorMessage);

  readonly approvals = computed(() => this.approvalsState().data);
  readonly approvalsLoading = computed(() => this.approvalsState().isLoading);
  readonly approvalsError = computed(() => this.approvalsState().errorMessage);

  readonly traceEvents = computed(() => this.traceState().data);
  readonly traceLoading = computed(() => this.traceState().isLoading);
  readonly traceError = computed(() => this.traceState().errorMessage);

  async loadDecisions(take = 50): Promise<void> {
    this.decisionsState.set({
      isLoading: true,
      errorMessage: '',
      data: this.decisionsState().data
    });

    try {
      const result = await firstValueFrom(this.api.getAiDecisions(take));

      this.decisionsState.set({
        isLoading: false,
        errorMessage: '',
        data: result ?? []
      });
    } catch (err: any) {
      this.decisionsState.set({
        isLoading: false,
        errorMessage: err?.error?.message || err?.message || 'Failed to load AI decisions.',
        data: []
      });
    }
  }

  async loadDecisionDetail(aiDecisionId: number): Promise<void> {
    this.decisionDetailState.set({
      isLoading: true,
      errorMessage: '',
      data: null
    });

    try {
      const result = await firstValueFrom(this.api.getAiDecisionById(aiDecisionId));

      this.decisionDetailState.set({
        isLoading: false,
        errorMessage: '',
        data: result
      });
    } catch (err: any) {
      this.decisionDetailState.set({
        isLoading: false,
        errorMessage: err?.status === 404
          ? `Decision ${aiDecisionId} was not found.`
          : err?.error?.message || err?.message || 'Failed to load decision detail.',
        data: null
      });
    }
  }

  async loadPendingApprovals(): Promise<void> {
    this.approvalsState.set({
      isLoading: true,
      errorMessage: '',
      data: this.approvalsState().data
    });

    try {
      const result = await firstValueFrom(this.api.getPendingApprovals());

      this.approvalsState.set({
        isLoading: false,
        errorMessage: '',
        data: result ?? []
      });
    } catch (err: any) {
      this.approvalsState.set({
        isLoading: false,
        errorMessage: err?.error?.message || err?.message || 'Failed to load pending approvals.',
        data: []
      });
    }
  }

  async approveApproval(approvalRequestId: number, reviewedBy: string, reviewComments?: string): Promise<boolean> {
    this.approvalsState.update(state => ({
      ...state,
      errorMessage: ''
    }));

    try {
      await firstValueFrom(this.api.approveApprovalRequest(approvalRequestId, {
        reviewedBy,
        reviewComments
      }));

      await this.loadPendingApprovals();
      return true;
    } catch (err: any) {
      this.approvalsState.update(state => ({
        ...state,
        errorMessage: err?.error?.message || err?.message || 'Approve failed.'
      }));
      return false;
    }
  }

  async rejectApproval(approvalRequestId: number, reviewedBy: string, reviewComments?: string): Promise<boolean> {
    this.approvalsState.update(state => ({
      ...state,
      errorMessage: ''
    }));

    try {
      await firstValueFrom(this.api.rejectApprovalRequest(approvalRequestId, {
        reviewedBy,
        reviewComments
      }));

      await this.loadPendingApprovals();
      return true;
    } catch (err: any) {
      this.approvalsState.update(state => ({
        ...state,
        errorMessage: err?.error?.message || err?.message || 'Reject failed.'
      }));
      return false;
    }
  }

  async loadTrace(correlationId: string): Promise<void> {
    this.traceState.set({
      isLoading: true,
      errorMessage: '',
      data: this.traceState().data
    });

    try {
      const result = await firstValueFrom(this.api.getSystemEventsByCorrelationId(correlationId));

      this.traceState.set({
        isLoading: false,
        errorMessage: '',
        data: result ?? []
      });
    } catch (err: any) {
      this.traceState.set({
        isLoading: false,
        errorMessage: err?.error?.message || err?.message || 'Failed to load trace.',
        data: []
      });
    }
  }

  clearDecisionDetail(): void {
    this.decisionDetailState.set({
      isLoading: false,
      errorMessage: '',
      data: null
    });
  }
}