export default class MarkdownRenderer extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      copy: ""
    }
  }

  componentDidMount() {
    this.fetch()
  }

  fetch = () => {
    if (this.state.copy) {
      // Clear current state, then refetch data
      this.setState({copy: ""}, this.fetch)
      return;
    }
    let asset = Expo.Asset.fromModule(md)
    const id = Math.floor(Math.random()  * 100) % 40;
    console.log(`[${id}] Started fetching data`, asset.localUri)
    let start = new Date(), end;

    const save = (res) => {
      this.setState({copy: res})
      let end = new Date();
      console.info(`[${id}] Completed fetching data in ${(end - start) / 1000} seconds`)
    }

    // Using Expo.FileSystem.readAsStringAsync.
    // Makes it a single asynchronous call, but must always use localUri
    // Therefore, downloadAsync is required
    let method1 = () => {
      if (!asset.localUri) {
        asset.downloadAsync().then(()=>{
          Expo.FileSystem.readAsStringAsync(asset.localUri).then(save)
        })
      } else {
        Expo.FileSystem.readAsStringAsync(asset.localUri).then(save)
      }
    }

    // Use fetch ensuring the usage of a localUri
    let method2 = () => {
      if (!asset.localUri) {
        asset.downloadAsync().then(()=>{
          fetch(asset.localUri).then(res => res.text()).then(save)
        })
      } else {
        fetch(asset.localUri).then(res => res.text()).then(save)
      }
    }

    // Use fetch but using `asset.uri` (not the local file)
    let method3 = () => {
      fetch(asset.uri).then(res => res.text()).then(save)
    }

    // method1()
    // method2()
    method3()
  }

  changeText = () => {
    let asset = Expo.Asset.fromModule(md)
    Expo.FileSystem.writeAsStringAsync(asset.localUri, "Hello World");
  }

  render() {
    return (
        <ScrollView style={{maxHeight: "90%"}}>
          <Button onPress={this.fetch} title="Refetch"/>
          <Button onPress={this.changeText} title="Change Text"/>
            <Markdown>{this.state.copy}</Markdown>
        </ScrollView>
    );
  }
}