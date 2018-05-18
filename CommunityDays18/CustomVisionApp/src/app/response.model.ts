interface FrameResponse {
    created: Date;
    id: string;
    iteration: string;
    predictions: [
        {
            boundingBox: any;
            probability: number;
            tagId: string;
            tagName: string;
        }
    ];
    project: string;
}
