# udping
A simple tool to show RTT of udp packet

Usage：udping.exe [ip] [port] [interval]

IPV4 only

强迫症用的

参考：https://github.com/wangyu-/UDPping

TODO:没有设计丢包，到时候再看看罢

# Getting Started

### Step 1

Set up a udp echo server at the host you want to ping. 

There are many ways of doing this, my favourite way is:

```
socat -v UDP-LISTEN:4000,fork PIPE
```

Now a echo server is listening at port 4000. 

###### Note
If you dont have socat, use `apt install socat` or `yum install socat`, you will get it.

### Step 2

Ping you server.

Assume `44.55.66.77` is the IP of your server.

```
udping 44.55.66.77 4000
```

Done!

