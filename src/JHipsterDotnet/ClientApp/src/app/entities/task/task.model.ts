import { IJob } from "app/entities/job/job.model";

export interface ITask {
  id?: number;
  title?: string | null;
  description?: string | null;
  jobs?: IJob[] | null;
}

export class Task implements ITask {
  constructor(
    public id?: number,
    public title?: string | null,
    public description?: string | null,
    public jobs?: IJob[] | null
  ) {}
}

export function getTaskIdentifier(task: ITask): number | undefined {
  return task.id;
}
