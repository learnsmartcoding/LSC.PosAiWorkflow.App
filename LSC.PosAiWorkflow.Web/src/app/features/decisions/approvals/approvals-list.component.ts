import { DatePipe } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { WorkflowUiStore } from '../../../core/services/workflow-ui.store';


@Component({
  selector: 'app-approvals-list',
  standalone: true,
  imports: [FormsModule, RouterLink, DatePipe],
  templateUrl: './approvals-list.component.html'
})
export class ApprovalsListComponent implements OnInit {
  readonly store = inject(WorkflowUiStore);

  readonly approvals = computed(() => this.store.approvals());
  readonly isLoading = computed(() => this.store.approvalsLoading());
  readonly errorMessage = computed(() => this.store.approvalsError());

  readonly reviewerName = signal('Reviewer');
  readonly reviewComments = signal('');
  readonly actionInProgressId = signal<number | null>(null);

  ngOnInit(): void {
    void this.store.loadPendingApprovals();
  }

  refresh(): void {
    void this.store.loadPendingApprovals();
  }

  async approve(approvalRequestId: number): Promise<void> {
    this.actionInProgressId.set(approvalRequestId);

    await this.store.approveApproval(
      approvalRequestId,
      this.reviewerName(),
      this.reviewComments()
    );

    this.actionInProgressId.set(null);
  }

  async reject(approvalRequestId: number): Promise<void> {
    this.actionInProgressId.set(approvalRequestId);

    await this.store.rejectApproval(
      approvalRequestId,
      this.reviewerName(),
      this.reviewComments()
    );

    this.actionInProgressId.set(null);
  }
}