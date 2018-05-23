import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import FrameResponse from './response.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {

  private timer = 0;
  private context: CanvasRenderingContext2D | undefined;
  private lastTag = '';
  private working = false;

  mediaStream: MediaStream | undefined;
  description = 'Initializing...';
  tag = '';
  options: object;

  @ViewChild('canvas') canvas: ElementRef<HTMLCanvasElement>;
  @ViewChild('video') video: ElementRef<HTMLVideoElement>;
  @ViewChild('chart') chart: any;

  constructor() {
    // Inizializzazione del grafico
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
    // Fermo la camera
    if (this.mediaStream) {
      this.mediaStream.getTracks()[0].stop();
    }
    // Fermo la cattura dei fotogrammi
    clearInterval(this.timer);
    this.mediaStream = undefined;
  }

  start() {
    this.run(false);
  }

  async run(captureMode: boolean): Promise<void> {
    this.stop();

    // Attivo la camera predefinita
    this.mediaStream = await navigator.mediaDevices.getUserMedia({ video: true });
    if (this.mediaStream) {
      this.description = (captureMode) ? 'Capturing' : 'Running!';
    }

    const fn: any = (...args: any[]) => this.sendFrame(captureMode);
    // Attivo la cattura dei frame
    this.timer = setInterval(fn, 1000);
  }

  private sendFrame(captureMode: boolean) {
    if (this.working) {
      return;
    }

    // Ridimensiono il canvas quando la dimensione del video
    this.canvas.nativeElement.width = this.video.nativeElement.videoWidth;
    this.canvas.nativeElement.height = this.video.nativeElement.videoHeight;
    this.context.drawImage(this.video.nativeElement, 0, 0);

    // Catturo il fotogramma
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
      this.working = false;
    };
    xhr.send(b);
  }

  private evaluateImage(b: Blob) {
    const xhr = new XMLHttpRequest();
    const defaultPrediction = { probability: 0, tagName: 'none' };

    xhr.open('POST', 'http://localhost/image', true);
    xhr.setRequestHeader('Content-Type', 'application/octet-stream');
    xhr.onload = () => {
      this.working = false;

      const p = JSON.parse(xhr.response) as FrameResponse;
      if (!p.predictions) {
        p.predictions = [defaultPrediction];
      }
      // Cerco il tag con probabilità più alta
      let result = p.predictions
        .sort((x, y) => x.probability > y.probability ? -1 : 1)[0];
      // Controllo di sicurezza
      if (!result || result.probability < 0.1) {
        result = defaultPrediction;
      }

      // Popolo il grafico
      this.tag = result.tagName;
      const serie = this.chart.chart.series[0];
      if (serie.data.length > 50) {
        serie.removePoint();
      }
      serie.addPoint(result.probability * 100);

      console.log(this.lastTag + ' ' + result.tagName + ' ' + result.probability);

      // Notifico se è un nuovo tag
      if (this.lastTag !== this.tag) {
        this.lastTag = this.tag;
        if (this.tag !== 'none' && result.probability > 0.9) {
          this.uploadImageForNotification(b);
        }
      }
    };
    xhr.send(b);
  }

  private uploadImageForNotification(b: Blob) {
    const xhr = new XMLHttpRequest();
    // tslint:disable-next-line:max-line-length
    xhr.open('PUT', 'https://ricciolo.blob.core.windows.net/cdays18/screenshot.jpg?st=2018-05-23T12%3A24%3A00Z&se=2019-05-24T12%3A24%3A00Z&sp=rw&sv=2017-04-17&sr=b&sig=XQ%2Bs667jqCLgEviSKHHYrlVs%2F5vPzxaXFNxggjzvgRc%3D', true);
    xhr.setRequestHeader('Content-Type', 'application/octet-stream');
    xhr.setRequestHeader('x-ms-blob-type', 'BlockBlob');
    xhr.onload = () => {
      this.notify();
    };
    xhr.send(b);
  }

  private notify() {
    const xhr = new XMLHttpRequest();
    // tslint:disable-next-line:max-line-length
    xhr.open('POST', 'https://cdays18fn.azurewebsites.net/api/HttpTriggerCSharp1?code=E4u89u/UaIFgsGxT63/N4dRkrnts7QDDjYEcmKwJY05uQvN3NBRi8A==', true);
    xhr.onload = () => {
    };
    // tslint:disable-next-line:max-line-length
    xhr.send('{"ServiceID":"urn:micasaverde-com:serviceId:HomeAutomationGateway1","DeviceID":0,"Command":"RunScene","CommandParameter":"SceneNum","Value":"777","Username":"","Action":"action","ActionCommand":"https://ricciolo.blob.core.windows.net/cdays18/screenshot.jpg"}');
  }
}
