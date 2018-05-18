import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {

  timer = 0;
  context: CanvasRenderingContext2D | undefined;
  mediaStream: MediaStream | undefined;
  description = 'Initializing...';
  tag = '';
  options: object;

  @ViewChild('canvas') canvas: ElementRef<HTMLCanvasElement>;
  @ViewChild('video') video: ElementRef<HTMLVideoElement>;
  @ViewChild('chart') chart: any;

  constructor() {
    this.options = {
      title: { text: 'Probability' },
      yAxis: {
        max: 100,
        min: 0
      },
      series: [{
        name: 'values',
        data: [0]
      }]
    };
  }

  ngOnInit() {
    this.context = this.canvas.nativeElement.getContext('2d');

    this.start();
  }

  capture() {
    this.run(true);
  }

  stop() {
    this.description = 'Stop';
    if (this.mediaStream) {
      this.mediaStream.getTracks()[0].stop();
    }
    clearInterval(this.timer);
    this.mediaStream = undefined;
  }

  start() {
    this.run(false);
  }

  async run(captureMode: boolean): Promise<void> {
    this.stop();

    this.mediaStream = await navigator.mediaDevices.getUserMedia({ video: true });
    if (this.mediaStream) {
      this.description = (captureMode) ? 'Capturing' : 'Running!';
    }

    const fn: any = (...args: any[]) => this.sendFrame(captureMode);
    this.timer = setInterval(fn, 200);
  }

  private sendFrame(captureMode: boolean) {
    this.canvas.nativeElement.width = this.video.nativeElement.videoWidth;
    this.canvas.nativeElement.height = this.video.nativeElement.videoHeight;
    this.context.drawImage(this.video.nativeElement, 0, 0);

    this.canvas.nativeElement.toBlob(b => {
      if (!captureMode) {
        this.evaluateImage(b);
      } else {
        this.uploadImage(b);
      }
    }, 'image/jpeg');
  }

  private uploadImage(b: Blob) {
    const projectId = 'c1bd788f-89a0-4b50-8d9b-d417f5341af1';
    const trainingKey = 'eef8b8b1dbbb4565956a69ee1d9c872e';

    const xhr = new XMLHttpRequest();
    xhr.open('POST', `https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Training/projects/${projectId}/images`, true);
    xhr.setRequestHeader('Content-Type', 'application/octet-stream');
    xhr.setRequestHeader('Training-Key', trainingKey);
    xhr.onload = () => {
      console.log(xhr.status);
    };
    xhr.send(b);
  }

  private evaluateImage(b: Blob) {
    const xhr = new XMLHttpRequest();
    xhr.open('POST', 'http://localhost/image', true);
    xhr.setRequestHeader('Content-Type', 'application/octet-stream');
    xhr.onload = () => {
      const p = JSON.parse(xhr.response) as FrameResponse;
      if (p.predictions) {
        const result = p.predictions
          .sort((x, y) => x.probability > y.probability ? -1 : 1)[0];

        this.tag = result.tagName;
        const serie = this.chart.chart.series[0];
        if (serie.data.length > 50) {
          serie.removePoint();
        }
        serie.addPoint(result.probability * 100);

        console.log(result.tagName + ' ' + result.probability);
      }
    };
    xhr.send(b);
  }
}
