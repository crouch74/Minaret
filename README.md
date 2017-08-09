# Minaret

Akka .NET seed node.

## Getting Started

Minaret is a seed node for Akka .NET cluser, it's like what [lighthouse](https://github.com/petabridge/lighthouse) is trying to do.
But it also exposes an api to view and control cluster nodes.


## Configuration

Inside akka HOCON config section you need to edit the following values.

```
minaret {
                system = "YourSystemName" 
                api-port = "1234"
          }
```
and this seed node address
```
cluster {
    seed-nodes = ["akka.tcp://YourSystemName@localhost:4035"]
}
```

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details
