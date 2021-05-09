import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { IJobHistory } from "../job-history.model";
import { JobHistoryService } from "../service/job-history.service";
import { JobHistoryDeleteDialogComponent } from "../delete/job-history-delete-dialog.component";

@Component({
  selector: "jhi-job-history",
  templateUrl: "./job-history.component.html",
})
export class JobHistoryComponent implements OnInit {
  jobHistories?: IJobHistory[];
  isLoading = false;

  constructor(
    protected jobHistoryService: JobHistoryService,
    protected modalService: NgbModal
  ) {}

  loadAll(): void {
    this.isLoading = true;

    this.jobHistoryService.query().subscribe(
      (res: HttpResponse<IJobHistory[]>) => {
        this.isLoading = false;
        this.jobHistories = res.body ?? [];
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  ngOnInit(): void {
    this.loadAll();
  }

  trackId(index: number, item: IJobHistory): number {
    return item.id!;
  }

  delete(jobHistory: IJobHistory): void {
    const modalRef = this.modalService.open(JobHistoryDeleteDialogComponent, {
      size: "lg",
      backdrop: "static",
    });
    modalRef.componentInstance.jobHistory = jobHistory;
    // unsubscribe not needed because closed completes on modal close
    modalRef.closed.subscribe((reason) => {
      if (reason === "deleted") {
        this.loadAll();
      }
    });
  }
}
