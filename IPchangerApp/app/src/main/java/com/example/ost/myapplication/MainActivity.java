package com.example.ost.myapplication;


import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.math.BigInteger;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.net.Socket;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.Enumeration;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.net.wifi.WifiManager;
import android.nfc.Tag;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.app.Activity;
import android.provider.Settings;
import android.telephony.SubscriptionManager;
import android.telephony.TelephonyManager;
import android.text.format.Formatter;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.appindexing.Action;
import com.google.android.gms.appindexing.AppIndex;
import com.google.android.gms.common.api.GoogleApiClient;

public class MainActivity extends Activity {

    TextView textResponse;
    EditText editTextAddress, editTextPort;
    Button buttonConnect, buttonClear, buttonClose;
    Socket socket = null;
    MyClientTask myClientTask;
    /**
     * ATTENTION: This was auto-generated to implement the App Indexing API.
     * See https://g.co/AppIndexing/AndroidStudio for more information.
     */
    private GoogleApiClient client;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        editTextAddress = (EditText) findViewById(R.id.address);
        editTextPort = (EditText) findViewById(R.id.port);
        buttonConnect = (Button) findViewById(R.id.connect);
        buttonClear = (Button) findViewById(R.id.clear);
        buttonClose = (Button) findViewById(R.id.close);
        textResponse = (TextView) findViewById(R.id.response);


        buttonConnect.setOnClickListener(buttonConnectOnClickListener);
        buttonClose.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    if (socket != null) {
                        socket.close();
                        textResponse.append("소켓 종료..\n");
                    }
                } catch (IOException e) {
                    textResponse.append(e.toString() + "\n");
                    e.printStackTrace();
                }
                try {
                    myClientTask.cancel(true);
                } catch (Exception e) {
                    textResponse.append(e.toString() + "\n쓰레드 종료 에러1");
                    textResponse.append("쓰레드 종료 에러2");
                }


                textResponse.append("접속 종료..\n");
            }
        });
        buttonClear.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View v) {
                textResponse.setText("");
            }
        });
        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        client = new GoogleApiClient.Builder(this).addApi(AppIndex.API).build();


        runOnUiThread(
                new Runnable() {
                    @Override
                    public void run() {
                        textResponse.append("아이피 주소..\n");
                        textResponse.append(getLocalIpAddress() + "\n");
                    }
                });

    }

    OnClickListener buttonConnectOnClickListener =
            new OnClickListener() {

                @Override
                public void onClick(View arg0) {

                    try {
                        myClientTask.cancel(true);

                    } catch (Exception e) {
                        textResponse.append(e.toString() + "\n");
                        textResponse.append("쓰레드 종료 에러");
                    }

                    textResponse.append("연결 실행..\n");
                    myClientTask = new MyClientTask(
                            editTextAddress.getText().toString(),
                            Integer.parseInt(editTextPort.getText().toString()));
                    textResponse.append("쓰레드 클래스 생성\n");
                    myClientTask.execute();

                }
            };


    @Override
    public void onStart() {
        super.onStart();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        client.connect();
        Action viewAction = Action.newAction(
                Action.TYPE_VIEW, // TODO: choose an action type.
                "Main Page", // TODO: Define a title for the content shown.
                // TODO: If you have web page content that matches this app activity's content,
                // make sure this auto-generated web page URL is correct.
                // Otherwise, set the URL to null.
                Uri.parse("http://host/path"),
                // TODO: Make sure this auto-generated app deep link URI is correct.
                Uri.parse("android-app://com.example.ost.myapplication/http/host/path")
        );
        AppIndex.AppIndexApi.start(client, viewAction);
    }

    @Override
    public void onStop() {
        super.onStop();

        // ATTENTION: This was auto-generated to implement the App Indexing API.
        // See https://g.co/AppIndexing/AndroidStudio for more information.
        Action viewAction = Action.newAction(
                Action.TYPE_VIEW, // TODO: choose an action type.
                "Main Page", // TODO: Define a title for the content shown.
                // TODO: If you have web page content that matches this app activity's content,
                // make sure this auto-generated web page URL is correct.
                // Otherwise, set the URL to null.
                Uri.parse("http://host/path"),
                // TODO: Make sure this auto-generated app deep link URI is correct.
                Uri.parse("android-app://com.example.ost.myapplication/http/host/path")
        );
        AppIndex.AppIndexApi.end(client, viewAction);
        client.disconnect();
    }

    public class MyClientTask extends AsyncTask<Void, Void, Void> {

        String dstAddress;
        int dstPort;
        String response = "";

        MyClientTask(String addr, int port) {
            dstAddress = addr;
            dstPort = port;
        }

        @Override
        protected Void doInBackground(Void... arg0) {


            try {


                runOnUiThread(
                        new Runnable() {
                            @Override
                            public void run() {
                                textResponse.append("접속중..\n");
                            }
                        });
                socket = new Socket(dstAddress, dstPort);
                OutputStream out;
                InputStream in;
                PrintWriter pw;
                BufferedReader br;
                out = socket.getOutputStream();
                in = socket.getInputStream();
                pw = new PrintWriter(new OutputStreamWriter(out));
                br = new BufferedReader(new InputStreamReader(in));
                /*
                ByteArrayOutputStream byteArrayOutputStream =
                        new ByteArrayOutputStream(1024);
                byte[] buffer = new byte[1024];

                int bytesRead;
                InputStream inputStream = socket.getInputStream();
                */
    /*
     * notice:
     * inputStream.read() will block if no data return
     */
                runOnUiThread(
                        new Runnable() {
                            @Override
                            public void run() {
                                textResponse.append("접속 성공(OK)..\n");
                            }
                        });
                String line = null;
                if ((line = br.readLine()) != null) {
                    System.out.println(line);


                    if (line.startsWith("SSH") || line.startsWith("change")) {
                        //데이터가 켜질 때 까지 무한 루프 켜지면 끄기
                        while (!isMobileDataEnabledFromLollipop(getApplicationContext())) {
                        }

                        runOnUiThread(
                                new Runnable() {
                                    @Override
                                    public void run() {
                                        textResponse.append("아이피 주소..BEFORE\n");
                                        textResponse.append(getLocalIpAddress() + "\n");
                                    }
                                });

                        setMobileNetworkfromLollipop(getApplicationContext());

                        runOnUiThread(
                                new Runnable() {
                                    @Override
                                    public void run() {
                                        textResponse.append("아이피 주소..AFTER\n");
                                        textResponse.append(getLocalIpAddress() + "\n");
                                    }
                                });

                    } else if (line.startsWith("close")) {
                        socket.close();

                    }


                    final String finalLine = line;
                    runOnUiThread(
                            new Runnable() {
                                @Override
                                public void run() {
                                    textResponse.append(finalLine + "\n");
                                }
                            });
                }
                /*while ((bytesRead = inputStream.read(buffer)) != -1){
                    byteArrayOutputStream.write(buffer, 0, bytesRead);
                    response += byteArrayOutputStream.toString("UTF-8");
                }*/

            } catch (Exception e) {
                //Log.(e.toString());
                response = "IOException: " + e.toString();
                try {

                } catch (Exception e1) {
                    e1.printStackTrace();
                }

            } finally {
                if (socket != null) {
                    try {
                        socket.close();
                    } catch (IOException e) {
                        // TODO Auto-generated catch block
                        e.printStackTrace();
                        response = "" + e.toString();
                    }
                }

                try {
                    //켜져 있으면 무한루프 꺼져 있으면 켜기
                    while (isMobileDataEnabledFromLollipop(getApplicationContext())) {
                    }
                    setMobileNetworkfromLollipop(getApplicationContext());

                    System.out.println("working!!");
                    runOnUiThread(
                            new Runnable() {
                                @Override
                                public void run() {
                                    textResponse.append("waiting..." + "\n");
                                }
                            });
                    Thread.sleep(1000 * 10);

                    if (!isCancelled()) {
                        myClientTask = new MyClientTask(
                                dstAddress,
                                dstPort);
                        myClientTask.execute();
                    }

//
//                        myClientTask = new MyClientTask(
//                                dstAddress,
//                                dstPort);
//                        myClientTask.execute();


                } catch (Exception e1) {
                    e1.printStackTrace();
                }

            }
            return null;
        }


        @Override
        protected void onPostExecute(Void result) {
            textResponse.setText(response + "\n");
            super.onPostExecute(result);
        }


    }


    public static void setMobileNetworkfromLollipop(Context context) throws Exception {
        String command = null;
        int state = 0;
        try {
            // Get the current state of the mobile network.
            state = isMobileDataEnabledFromLollipop(context) ? 0 : 1;
            // Get the value of the "TRANSACTION_setDataEnabled" field.
            String transactionCode = getTransactionCode(context);
            // Android 5.1+ (API 22) and later.
            if (Build.VERSION.SDK_INT > Build.VERSION_CODES.LOLLIPOP) {
                SubscriptionManager mSubscriptionManager = (SubscriptionManager) context.getSystemService(Context.TELEPHONY_SUBSCRIPTION_SERVICE);
                // Loop through the subscription list i.e. SIM list.
                for (int i = 0; i < mSubscriptionManager.getActiveSubscriptionInfoCountMax(); i++) {
                    if (transactionCode != null && transactionCode.length() > 0) {
                        // Get the active subscription ID for a given SIM card.
                        int subscriptionId = mSubscriptionManager.getActiveSubscriptionInfoList().get(i).getSubscriptionId();
                        // Execute the command via `su` to turn off
                        // mobile network for a subscription service.
                        command = "service call phone " + transactionCode + " i32 " + subscriptionId + " i32 " + state;
                        System.out.println(command);
                        executeCommandViaSu(context, "-c", command);
                    }
                }
            } else if (Build.VERSION.SDK_INT == Build.VERSION_CODES.LOLLIPOP) {
                // Android 5.0 (API 21) only.
                if (transactionCode != null && transactionCode.length() > 0) {
                    // Execute the command via `su` to turn off mobile network.
                    command = "service call phone " + transactionCode + " i32 " + state;
                    System.out.println(command);
                    executeCommandViaSu(context, "-c", command);
                }
            }
        } catch (Exception e) {
            // Oops! Something went wrong, so we throw the exception here.
            throw e;
        }
    }

    public String getLocalIpAddress() {
        try {
            for (Enumeration<NetworkInterface> en = NetworkInterface.getNetworkInterfaces(); en.hasMoreElements(); ) {
                NetworkInterface intf = en.nextElement();
                for (Enumeration<InetAddress> enumIpAddr = intf.getInetAddresses(); enumIpAddr.hasMoreElements(); ) {
                    InetAddress inetAddress = enumIpAddr.nextElement();
                    if (!inetAddress.isLoopbackAddress()) {
                        return inetAddress.getHostAddress().toString();
                    }
                }
            }
        } catch (SocketException ex) {
            Log.e("tag", ex.toString());
        }
        return null;
    }


    private static boolean isMobileDataEnabledFromLollipop(Context context) {
        boolean state = false;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            state = Settings.Global.getInt(context.getContentResolver(), "mobile_data", 0) == 1;
        }
        return state;
    }

    private static String getTransactionCode(Context context) throws Exception {
        try {
            final TelephonyManager mTelephonyManager = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
            final Class<?> mTelephonyClass = Class.forName(mTelephonyManager.getClass().getName());
            final Method mTelephonyMethod = mTelephonyClass.getDeclaredMethod("getITelephony");
            mTelephonyMethod.setAccessible(true);
            final Object mTelephonyStub = mTelephonyMethod.invoke(mTelephonyManager);
            final Class<?> mTelephonyStubClass = Class.forName(mTelephonyStub.getClass().getName());
            final Class<?> mClass = mTelephonyStubClass.getDeclaringClass();
            final Field field = mClass.getDeclaredField("TRANSACTION_setDataEnabled");
            field.setAccessible(true);
            return String.valueOf(field.getInt(null));
        } catch (Exception e) {
            // The "TRANSACTION_setDataEnabled" field is not available,
            // or named differently in the current API level, so we throw
            // an exception and inform users that the method is not available.
            throw e;
        }
    }

    private static void executeCommandViaSu(Context context, String option, String command) {
        boolean success = false;
        String su = "su";
        for (int i = 0; i < 3; i++) {
            // Default "su" command executed successfully, then quit.
            if (success) {
                break;
            }
            // Else, execute other "su" commands.
            if (i == 1) {
                su = "/system/xbin/su";
            } else if (i == 2) {
                su = "/system/bin/su";
            }
            try {
                // Execute command as "su".
                Runtime.getRuntime().exec(new String[]{su, option, command});
            } catch (IOException e) {
                success = false;
                // Oops! Cannot execute `su` for some reason.
                // Log error here.
            } finally {
                success = true;
            }
        }
    }


}
