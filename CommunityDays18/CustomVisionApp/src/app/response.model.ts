export default interface FrameResponse {
    created: Date;
    id: string;
    iteration: string;
    predictions: [
        {
            probability: number;
            tagName: string;
        }
    ];
    project: string;
}
