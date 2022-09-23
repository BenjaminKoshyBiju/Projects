package test.proximityapp;

import androidx.appcompat.app.AppCompatActivity;

import android.graphics.Color;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.speech.tts.TextToSpeech;
import android.widget.TextView;
import android.widget.Toast;

import java.util.Locale;

public class MainActivity extends AppCompatActivity {

    TextToSpeech t2;

    private SensorManager sensorManager;
    private Sensor proximitySensor;
    private SensorEventListener proximitySensorListner;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        t2=new TextToSpeech(getApplicationContext(), new TextToSpeech.OnInitListener() {
            public void onInit(int status) {
                if(status != TextToSpeech.ERROR) {
                    t2.setLanguage(Locale.UK);

                }
            }
        });

        sensorManager= (SensorManager) getSystemService(SENSOR_SERVICE);
        proximitySensor=sensorManager.getDefaultSensor(Sensor.TYPE_PROXIMITY);
        Sensor sensorShake=sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
        if(proximitySensor==null)
        {
            Toast.makeText(this, "Sensor is not available", Toast.LENGTH_SHORT).show();
            finish();
        }
        SensorEventListener sensorEventListener=new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent sensorEvent) {
                TextView tShake;
                tShake=(TextView) findViewById(R.id.tv);
                if(sensorEvent!=null)
                {
                    float x_accl=sensorEvent.values[0];
                    float y_accl=sensorEvent.values[1];
                    float z_accl=sensorEvent.values[2];
                    float floatSum=Math.abs(x_accl)+Math.abs(y_accl)+Math.abs(z_accl);
                  //  if(x_accl>2 || x_accl<-2 || y_accl>12 || y_accl<-12 || z_accl>2 || z_accl<-2)
                    if(floatSum>15)
                    {
                        tShake.setText("Please stop shaking!");
                    }
                    else
                    {
                        tShake.setText("");
                    }
                }

            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int i) {


            }
        };
        sensorManager.registerListener(sensorEventListener,sensorShake,SensorManager.SENSOR_DELAY_NORMAL);
        proximitySensorListner=new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent sensorEvent) {
                TextView t1;

                String str;

                t1=(TextView) findViewById(R.id.tv1);
                if(sensorEvent.values[0]< proximitySensor.getMaximumRange())
                {
                      str = "You are too close";
                    t1.setText("You are too close");
                    getWindow().getDecorView().setBackgroundColor(Color.RED);
                    t2.speak(str, TextToSpeech.QUEUE_FLUSH, null);
                }
                else
                {
                     str = "Bring your hands Closer";
                    t1.setText("Bring your hands Closer");
                    getWindow().getDecorView().setBackgroundColor(Color.GREEN);
                    t2.speak(str, TextToSpeech.QUEUE_FLUSH, null);
                }
            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int i) {

            }
        };
        sensorManager.registerListener(proximitySensorListner,proximitySensor,2*1000*1000);
    }
    @Override
    protected void onPause()
    {
        super.onPause();
        sensorManager.unregisterListener(proximitySensorListner);
    }
}